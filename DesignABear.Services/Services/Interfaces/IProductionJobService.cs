using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IProductionJobService
    {
        Task<IEnumerable<ProductionJobResponse>> GetByStatusAsync(string status);
        Task<IEnumerable<ProductionJobResponse>> GetByTechnicianAsync(Guid technicianId);
        Task<ProductionJobResponse?> GetByIdAsync(Guid jobId);
        Task<ProductionJobResponse?> GetByOrderItemAsync(Guid orderItemId);
        Task<ProductionJobResponse> CreateAsync(CreateProductionJobRequest request);
        Task UpdateStatusAsync(Guid jobId, string status, Guid updatedBy);
        Task AssignTechnicianAsync(Guid jobId, Guid technicianId);
        Task AssignPartAsync(Guid jobId, string partType, Guid technicianId, Guid updatedBy);
        Task UpdatePartStatusAsync(Guid jobId, string partType, string status, Guid updatedBy);
        Task HandoverPartAsync(Guid jobId, string partType, IEnumerable<string> photoUrls, string? note, Guid staffUserId);
        Task SubmitInspectionAsync(Guid jobId, Guid qcUserId, int score, string? note, bool isApproved, string? rejectedPartType = null);
        Task<IEnumerable<ProductionJobResponse>> GetLobbyAsync();
        Task ClaimFromLobbyAsync(Guid jobId, Guid technicianId);
    }
}
