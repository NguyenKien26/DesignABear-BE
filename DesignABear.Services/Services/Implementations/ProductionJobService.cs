using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using System.Text.Json;

namespace DesignABear.Services.Services.Implementations
{
    public class ProductionJobService : IProductionJobService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        private const decimal LaborRate = 0.20m;
        private const decimal CraftsmanShareRate = 0.70m;
        private const decimal QcShareRate = 0.30m;

        public ProductionJobService(IUnitOfWork uow, IMapper mapper, IEmailService emailService)
        {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<IEnumerable<ProductionJobResponse>> GetByStatusAsync(string status)
        {
            var jobs = await _uow.ProductionJobs.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<ProductionJobResponse>>(jobs);
        }

        public async Task<IEnumerable<ProductionJobResponse>> GetByTechnicianAsync(Guid technicianId)
        {
            var jobs = await _uow.ProductionJobs.GetByTechnicianAsync(technicianId);
            return _mapper.Map<IEnumerable<ProductionJobResponse>>(jobs);
        }

        public async Task<ProductionJobResponse?> GetByIdAsync(Guid jobId)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);
            return _mapper.Map<ProductionJobResponse>(job);
        }

        public async Task<ProductionJobResponse?> GetByOrderItemAsync(Guid orderItemId)
        {
            var job = await _uow.ProductionJobs.GetByOrderItemAsync(orderItemId);
            if (job == null) throw new NotFoundException("ProductionJob for OrderItem", orderItemId);
            return _mapper.Map<ProductionJobResponse>(job);
        }

        public async Task<ProductionJobResponse> CreateAsync(CreateProductionJobRequest request)
        {
            var orderItem = await _uow.OrderItems.GetByIdAsync(request.OrderItemId);
            if (orderItem == null)
            {
                throw new NotFoundException("OrderItem", request.OrderItemId);
            }

            var bearValue = orderItem.LineTotal > 0
                ? orderItem.LineTotal
                : orderItem.UnitPrice * Math.Max(orderItem.Quantity, 1);

            if (bearValue <= 0)
            {
                throw new BadRequestException("Giá trị order item phải lớn hơn 0 để tính commission.");
            }

            var totalAssemblyCost = Math.Round(bearValue * LaborRate, 0, MidpointRounding.AwayFromZero);
            var craftsmanCommission = Math.Round(totalAssemblyCost * CraftsmanShareRate, 0, MidpointRounding.AwayFromZero);
            var qcCommission = totalAssemblyCost - craftsmanCommission;

            var job = _mapper.Map<ProductionJob>(request);
            job.JobId = Guid.NewGuid();
            job.Status = ProductionStatus.InQueue;
            job.UpdatedAt = DateTime.UtcNow;

            job.TotalAssemblyCost = totalAssemblyCost;
            job.CraftsmanCommission = craftsmanCommission;
            job.QCCommission = qcCommission;

            await _uow.ProductionJobs.AddAsync(job);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ProductionJobResponse>(job);
        }

        public async Task UpdateStatusAsync(Guid jobId, string status, Guid updatedBy)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            job.Status = status.ToUpperInvariant();
            job.UpdatedAt = DateTime.UtcNow;
            job.UpdatedBy = updatedBy;

            if (job.Status == ProductionStatus.Finished) job.FinishedAt = DateTime.UtcNow;
            else if (job.Status == ProductionStatus.InProgress && job.StartedAt is null) job.StartedAt = DateTime.UtcNow;

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task AssignTechnicianAsync(Guid jobId, Guid technicianId)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            job.TechnicianId = technicianId;
            job.EspTechnicianId = technicianId;
            job.ShellTechnicianId = technicianId;
            job.Status = ProductionStatus.Assigned;
            job.UpdatedAt = DateTime.UtcNow;

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task AssignPartAsync(Guid jobId, string partType, Guid technicianId, Guid updatedBy)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            var normalizedPart = NormalizePartType(partType);

            if (normalizedPart == "ESP_CORE")
            {
                job.EspTechnicianId = technicianId;
                job.EspStatus = ProductionStatus.Assigned;
            }
            else
            {
                job.ShellTechnicianId = technicianId;
                job.ShellStatus = ProductionStatus.Assigned;
            }

            job.UpdatedBy = updatedBy;
            job.UpdatedAt = DateTime.UtcNow;
            SyncOverallStatus(job, DateTime.UtcNow);

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdatePartStatusAsync(Guid jobId, string partType, string status, Guid updatedBy)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            var normalizedPart = NormalizePartType(partType);
            var normalizedStatus = status.ToUpperInvariant();
            var now = DateTime.UtcNow;

            if (normalizedPart == "ESP_CORE")
            {
                job.EspStatus = normalizedStatus;
                if (normalizedStatus == ProductionStatus.Finished) job.EspFinishedAt = now;
            }
            else
            {
                job.ShellStatus = normalizedStatus;
                if (normalizedStatus == ProductionStatus.Finished) job.ShellFinishedAt = now;
            }

            job.UpdatedAt = now;
            job.UpdatedBy = updatedBy;
            SyncOverallStatus(job, now);

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task HandoverPartAsync(Guid jobId, string partType, IEnumerable<string> photoUrls, string? note, Guid staffUserId)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            var urls = photoUrls?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
            var normalizedPart = NormalizePartType(partType);
            var now = DateTime.UtcNow;

            var evidence = string.IsNullOrEmpty(job.EvidenceImages) 
                ? new List<object>() 
                : JsonSerializer.Deserialize<List<object>>(job.EvidenceImages) ?? new List<object>();
            
            if (normalizedPart == "ESP_CORE")
            {
                // This is the "Chip Source" confirmation
                job.EspStatus = ProductionStatus.Completed;
                job.EspHandoverNote = note;
                foreach (var url in urls)
                {
                    evidence.Add(new { tag = "CHIP_SOURCE", url = url, createdAt = now, uploadedBy = staffUserId });
                }
            }
            else
            {
                // This is the "Final Product" confirmation
                job.ShellStatus = ProductionStatus.Completed;
                job.ShellHandoverNote = note;
                foreach (var url in urls)
                {
                    evidence.Add(new { tag = "FINAL_PRODUCT", url = url, createdAt = now, uploadedBy = staffUserId });
                }
            }

            job.EvidenceImages = JsonSerializer.Serialize(evidence);

            job.UpdatedAt = now;
            job.UpdatedBy = staffUserId;
            SyncOverallStatus(job, now);

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task SubmitInspectionAsync(Guid jobId, Guid qcUserId, int score, string? note, bool isApproved, string? rejectedPartType = null)
        {
            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null) throw new NotFoundException("ProductionJob", jobId);

            job.QualityScore = score;
            job.QCInspectorId = qcUserId;
            job.QCHandoverNote = note;
            job.QCFinishedAt = DateTime.UtcNow;
            job.UpdatedBy = qcUserId;
            job.UpdatedAt = DateTime.UtcNow;

            if (isApproved)
            {
                job.Status = ProductionStatus.Finished;
                job.QCStatus = ProductionStatus.Completed;
                job.FinishedAt = DateTime.UtcNow;

                if (job.TechnicianId.HasValue)
                {
                    var craftsman = await _uow.Users.GetByIdAsync(job.TechnicianId.Value);
                    if (craftsman != null)
                    {
                        craftsman.TotalJobsCompleted += 1;
                        craftsman.TotalEarned += job.CraftsmanCommission;
                        _uow.Users.Update(craftsman);
                    }
                }

                var qcStaff = await _uow.Users.GetByIdAsync(qcUserId);
                if (qcStaff != null)
                {
                    qcStaff.TotalJobsCompleted += 1;
                    qcStaff.TotalEarned += job.QCCommission;
                    _uow.Users.Update(qcStaff);
                }

                await SyncOrderStatusAsync(job.OrderItemId);
                await NotifyCustomerShipmentAsync(job);
            }
            else
            {
                job.Status = ProductionStatus.ReworkRequired;
                job.QCStatus = ProductionStatus.ReworkRequired;
                job.RejectionReason = note;

                var rejectTarget = NormalizeRejectedPartType(rejectedPartType);

                if (rejectTarget == "ESP_CORE")
                {
                    job.EspStatus = ProductionStatus.ReworkRequired;
                }
                else if (rejectTarget == "PLUSH_SHELL")
                {
                    job.ShellStatus = ProductionStatus.ReworkRequired;
                }
                else // ALL
                {
                    job.EspStatus = ProductionStatus.ReworkRequired;
                    job.ShellStatus = ProductionStatus.ReworkRequired;
                }
            }

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductionJobResponse>> GetLobbyAsync()
        {
            var jobs = await _uow.ProductionJobs.GetByStatusAsync(ProductionStatus.InQueue);
            return _mapper.Map<IEnumerable<ProductionJobResponse>>(jobs);
        }

        public async Task ClaimFromLobbyAsync(Guid jobId, Guid technicianId)
        {
            if (technicianId == Guid.Empty)
                throw new BadRequestException("Invalid technician.");

            var job = await _uow.ProductionJobs.GetByIdAsync(jobId);
            if (job == null)
                throw new NotFoundException("ProductionJob", jobId);

            if (job.Status != ProductionStatus.InQueue)
                throw new BadRequestException($"Job {jobId} is not in lobby (IN_QUEUE). Current status: {job.Status}");

            if (job.TechnicianId.HasValue && job.TechnicianId.Value != technicianId)
                throw new BadRequestException("Job has been claimed by another craftsman.");

            job.TechnicianId = technicianId;

            if (!job.EspTechnicianId.HasValue) job.EspTechnicianId = technicianId;
            if (!job.ShellTechnicianId.HasValue) job.ShellTechnicianId = technicianId;

            if (job.EspStatus == ProductionStatus.InQueue) job.EspStatus = ProductionStatus.Assigned;
            if (job.ShellStatus == ProductionStatus.InQueue) job.ShellStatus = ProductionStatus.Assigned;

            job.Status = ProductionStatus.Assigned;
            job.UpdatedBy = technicianId;
            job.UpdatedAt = DateTime.UtcNow;

            _uow.ProductionJobs.Update(job);
            await _uow.SaveChangesAsync();
        }

        private async Task NotifyCustomerShipmentAsync(ProductionJob job)
        {
            try
            {
                var orderItem = await _uow.OrderItems.GetByIdAsync(job.OrderItemId);
                if (orderItem == null) return;

                var order = await _uow.Orders.GetWithDetailsAsync(orderItem.OrderId);
                if (order == null || order.User == null) return;

                string subject = $"[DesignABear] Chú gấu của bạn đã hoàn thành công đoạn sản xuất!";
                string body = $@"
                    <h2>Chúc mừng {order.User.FullName}!</h2>
                    <p>Chú gấu độc bản của bạn đã vượt qua vòng kiểm duyệt chất lượng gắt gao (QC Score: {job.QualityScore}/100).</p>
                    <p>Đơn hàng <b>#{order.OrderId.ToString().Substring(0, 8).ToUpper()}</b> hiện đang được bàn giao cho đơn vị vận chuyển GHTK-DEMO.</p>
                    <p>Chúng tôi sẽ sớm gửi mã vận đơn để bạn theo dõi hành trình của bé gấu.</p>
                    <br/>
                    <p>Trân trọng,</p>
                    <p>Đội ngũ DesignABear</p>";

                await _emailService.SendEmailAsync(order.User.Email, subject, body, true);
            }
            catch (Exception ex)
            {
                // Logic error in notification shouldn't rollback production status
                // Log and continue
            }
        }

        private static string NormalizePartType(string partType)
        {
            var value = (partType ?? string.Empty).Trim().ToUpperInvariant();
            return value switch
            {
                "ESP" => "ESP_CORE",
                "CORE" => "ESP_CORE",
                "ESP_CORE" => "ESP_CORE",
                _ => "PLUSH_SHELL"
            };
        }

        private static void SyncOverallStatus(ProductionJob job, DateTime now)
        {
            // If QC rejected it, we stay in ReworkRequired until at least one part is moved back to InProgress or Completed
            // However, once both parts are done again, we should move to AwaitingQC

            bool espDone = job.EspStatus == ProductionStatus.Completed || job.EspStatus == ProductionStatus.Finished;
            bool shellDone = job.ShellStatus == ProductionStatus.Completed || job.ShellStatus == ProductionStatus.Finished;

            if (espDone && shellDone)
            {
                job.Status = ProductionStatus.AwaitingQC;
                return;
            }

            if (job.EspStatus == ProductionStatus.InProgress || job.ShellStatus == ProductionStatus.InProgress)
            {
                job.Status = ProductionStatus.InProgress;
                return;
            }

            if (job.EspStatus == ProductionStatus.Assigned || job.ShellStatus == ProductionStatus.Assigned)
            {
                job.Status = ProductionStatus.Assigned;
                return;
            }

            job.Status = ProductionStatus.InQueue;
        }

        private async Task SyncOrderStatusAsync(Guid orderItemId)
        {
            var item = await _uow.OrderItems.GetByIdAsync(orderItemId);
            if (item == null) return;

            var order = await _uow.Orders.GetWithDetailsAsync(item.OrderId);
            if (order == null) return;

            // An order is READY_FOR_PICKUP only if ALL production jobs for ALL order items are FINISHED
            var allJobs = order.OrderItems.SelectMany(oi => oi.ProductionJobs).ToList();
            
            if (allJobs.Any() && allJobs.All(j => j.Status == ProductionStatus.Finished))
            {
                order.Status = OrderStatusConstants.ReadyForPickup;
                order.UpdatedAt = DateTime.UtcNow;
                _uow.Orders.Update(order);
            }
        }

        private static string NormalizeRejectedPartType(string? rejectedPartType)
        {
            var value = (rejectedPartType ?? string.Empty).Trim().ToUpperInvariant();

            return value switch
            {
                "ESP" => "ESP_CORE",
                "CORE" => "ESP_CORE",
                "ESP_CORE" => "ESP_CORE",
                "SHELL" => "PLUSH_SHELL",
                "PLUSH_SHELL" => "PLUSH_SHELL",
                "ALL" => "ALL",
                _ => "ALL"
            };
        }
    }
}
