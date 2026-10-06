using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionJobsController : BaseController
    {
        private readonly IProductionJobService _jobService;

        public ProductionJobsController(IProductionJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("lobby")]
        [Authorize(Roles = "1,2,4")]
        public async Task<IActionResult> GetLobby()
        {
            var jobs = await _jobService.GetLobbyAsync();
            return HandleResult(Result<IEnumerable<object>>.Success(jobs.Cast<object>()));
        }

        [HttpPost("{id}/claim")]
        [Authorize(Roles = "2,4")]
        public async Task<IActionResult> ClaimFromLobby(Guid id)
        {
            var userId = GetCurrentUserId();
            await _jobService.ClaimFromLobbyAsync(id, userId);
            return HandleResult(Result.Success());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null)
            {
                return HandleResult(Result<object>.Failure(new Error("NotFound", $"Production job with ID {id} not found.")));
            }
            return HandleResult(Result<object>.Success(job));
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var jobs = await _jobService.GetByStatusAsync(status);
            return HandleResult(Result<IEnumerable<object>>.Success(jobs.Cast<object>()));
        }

        [HttpGet("technician/{technicianId}")]
        public async Task<IActionResult> GetByTechnician(Guid technicianId)
        {
            var jobs = await _jobService.GetByTechnicianAsync(technicianId);
            return HandleResult(Result<IEnumerable<object>>.Success(jobs.Cast<object>()));
        }

        [HttpGet("orderitem/{orderItemId}")]
        public async Task<IActionResult> GetByOrderItem(Guid orderItemId)
        {
            var job = await _jobService.GetByOrderItemAsync(orderItemId);
            if (job == null)
            {
                return HandleResult(Result<object>.Failure(new Error("NotFound", $"Production job for order item {orderItemId} not found.")));
            }
            return HandleResult(Result<object>.Success(job));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductionJobRequest request)
        {
            var job = await _jobService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = job.JobId }, Result<object>.Success(job));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] string status, [FromQuery] Guid updatedBy)
        {
            await _jobService.UpdateStatusAsync(id, status, updatedBy);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignTechnician(Guid id, [FromQuery] Guid technicianId)
        {
            await _jobService.AssignTechnicianAsync(id, technicianId);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/assign-part")]
        [Authorize(Roles = "1,4,5")]
        public async Task<IActionResult> AssignPart(Guid id, [FromBody] AssignProductionPartRequest request)
        {
            var userId = GetCurrentUserId();
            await _jobService.AssignPartAsync(id, request.PartType, request.TechnicianId, userId);
            return HandleResult(Result.Success());
        }

        [HttpPut("{id}/part-status")]
        [Authorize(Roles = "1,2,4")]
        public async Task<IActionResult> UpdatePartStatus(Guid id, [FromBody] UpdateProductionPartStatusRequest request)
        {
            var userId = GetCurrentUserId();
            await _jobService.UpdatePartStatusAsync(id, request.PartType, request.Status, userId);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/handover")]
        [Authorize(Roles = "2,4")]
        public async Task<IActionResult> HandoverPart(Guid id, [FromBody] HandoverProductionPartRequest request)
        {
            var userId = GetCurrentUserId();
            await _jobService.HandoverPartAsync(id, request.PartType, request.PhotoUrls, request.Note, userId);
            return HandleResult(Result.Success());
        }

        [HttpPost("{id}/qc-inspection")]
        [Authorize(Roles = "1,2,5")]
        public async Task<IActionResult> SubmitQcInspection(Guid id, [FromBody] SubmitInspectionRequest request)
        {
            var qcUserId = GetCurrentUserId();
            await _jobService.SubmitInspectionAsync(
                id,
                qcUserId,
                request.Score,
                request.Notes,
                request.IsApproved,
                request.RejectedPartType);

            return HandleResult(Result.Success());
        }
    }
}
