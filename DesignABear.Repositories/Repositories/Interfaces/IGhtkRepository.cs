using DesignABear.Repositories.Integrations.Ghtk.Models;

namespace DesignABear.Repositories.Repositories.Interfaces
{
    public interface IGhtkRepository
    {
        Task<GhtkFeeResponse> CalculateFeeAsync(GhtkFeeRequest request, CancellationToken cancellationToken = default);
        Task<GhtkSubmitOrderResponse> SubmitExpressOrderAsync(GhtkSubmitOrderRequest request, CancellationToken cancellationToken = default);
        Task<GhtkPrintLabelFile> DownloadPrintLabelAsync(string label, CancellationToken cancellationToken = default);
        Task<GhtkTrackingStatusResponse> GetTrackingStatusAsync(string label, CancellationToken cancellationToken = default);
    }
}