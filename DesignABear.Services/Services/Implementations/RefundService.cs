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
    public class RefundService : IRefundService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RefundService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RefundResponse> CreateRequestAsync(RefundRequest request, Guid userId, int roleId)
        {
            var order = await _uow.Orders.GetByIdAsync(request.OrderId);
            if (order == null) throw new NotFoundException("Order", request.OrderId);

            var status = order.Status.ToUpper();
            if (status != "PAID" && status != "DELIVERED" && status != "COMPLETED")
            {
                throw new BadRequestException($"Refund cannot be requested for orders with status: {order.Status}. The order must be paid or delivered first.");
            }
            // Role 3 (Customer) can only request for their own orders
            if (roleId == 3 && order.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only request refunds for your own orders.");
            }

            if (status == "REFUND_PENDING")
            {
                throw new BadRequestException("A refund request is already pending for this order.");
            }

            var refund = new OrderRefund
            {
                RefundId = Guid.NewGuid(),
                OrderId = order.OrderId,
                RequestedBy = userId,
                Amount = request.Amount ?? order.GrandTotal,
                Reason = request.Reason,
                EvidenceUrls = request.EvidenceUrls != null ? JsonSerializer.Serialize(request.EvidenceUrls) : null,
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _uow.OrderRefunds.AddAsync(refund);
            
            // Update order status
            order.Status = "REFUND_PENDING";
            _uow.Orders.Update(order);
            
            await _uow.SaveChangesAsync();
            return _mapper.Map<RefundResponse>(refund);
        }

        public async Task<RefundResponse> ApproveAsync(Guid refundId, Guid userId, string? adminNotes)
        {
            var refund = await _uow.OrderRefunds.GetByIdAsync(refundId);
            if (refund == null) throw new NotFoundException("Refund", refundId);
            if (refund.Status != "PENDING") throw new BadRequestException($"Refund is already {refund.Status}");

            var order = await _uow.Orders.GetByIdAsync(refund.OrderId);
            if (order == null) throw new NotFoundException("Order", refund.OrderId);

            refund.Status = "APPROVED";
            refund.ApprovedBy = userId;
            refund.AdminNotes = adminNotes;
            refund.UpdatedAt = DateTime.UtcNow;

            _uow.OrderRefunds.Update(refund);

            // Update order status to waiting for execution OR finished if we just want to mark it
            order.Status = "REFUND_APPROVED";
            _uow.Orders.Update(order);

            await _uow.SaveChangesAsync();
            return _mapper.Map<RefundResponse>(refund);
        }

        public async Task<RefundResponse> RejectAsync(Guid refundId, Guid userId, string? adminNotes)
        {
            var refund = await _uow.OrderRefunds.GetByIdAsync(refundId);
            if (refund == null) throw new NotFoundException("Refund", refundId);
            if (refund.Status != "PENDING") throw new BadRequestException($"Refund is already {refund.Status}");

            var order = await _uow.Orders.GetByIdAsync(refund.OrderId);
            if (order == null) throw new NotFoundException("Order", refund.OrderId);

            refund.Status = "REJECTED";
            refund.ApprovedBy = userId;
            refund.AdminNotes = adminNotes;
            refund.UpdatedAt = DateTime.UtcNow;

            _uow.OrderRefunds.Update(refund);
            
            // Restore order status to PAID (or previous)
            order.Status = "PAID"; 
            _uow.Orders.Update(order);

            await _uow.SaveChangesAsync();
            return _mapper.Map<RefundResponse>(refund);
        }

        public async Task<IEnumerable<RefundResponse>> GetAllPendingAsync()
        {
            var refunds = await _uow.OrderRefunds.FindAsync(r => r.Status == "PENDING");
            return _mapper.Map<IEnumerable<RefundResponse>>(refunds);
        }

        public async Task<IEnumerable<RefundResponse>> GetRefundsByOrderAsync(Guid orderId)
        {
            var refunds = await _uow.OrderRefunds.FindAsync(r => r.OrderId == orderId);
            return _mapper.Map<IEnumerable<RefundResponse>>(refunds);
        }

        public async Task<RefundResponse?> GetByIdAsync(Guid refundId)
        {
            var refund = await _uow.OrderRefunds.GetByIdAsync(refundId);
            return _mapper.Map<RefundResponse>(refund);
        }
    }
}
