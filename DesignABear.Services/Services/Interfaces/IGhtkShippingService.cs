using DesignABear.Repositories.Integrations.Ghtk.Models;
using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;

namespace DesignABear.Services.Services.Interfaces
{
    public interface IGhtkShippingService
    {
        Task<Result<GhtkFeeResponse>> GetShippingFeeAsync(CalculateGhtkFeeRequest request, CancellationToken cancellationToken = default);
        Task<Result<GhtkFeeResponse>> CalculateFeeForCartAsync(Guid cartId, Guid addressId, string? transport = null, CancellationToken cancellationToken = default);
        Task<Result<GhtkFeeResponse>> CalculateFeeForOrderAsync(Guid orderId, string? transport = null, CancellationToken cancellationToken = default);
        Task<Result<GhtkSubmitOrderResponse>> SubmitExpressOrderAsync(SubmitGhtkExpressOrderRequest request, CancellationToken cancellationToken = default);
        Task<Result<GhtkPrintLabelFile>> DownloadPrintLabelAsync(string label, CancellationToken cancellationToken = default);
        Task<Result<GhtkTrackingStatusResponse>> GetTrackingStatusAsync(string label, CancellationToken cancellationToken = default);
    }
}