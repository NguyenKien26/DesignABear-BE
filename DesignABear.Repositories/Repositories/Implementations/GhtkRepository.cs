using System.Net;
using System.Text;
using System.Text.Json;
using DesignABear.Repositories.Integrations.Ghtk.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class GhtkRepository : IGhtkRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GhtkRepository> _logger;
        private readonly string _submitExpressOrderPath;
        private readonly string _printLabelPathTemplate;
        private readonly string _trackingStatusPathTemplate;

        public GhtkRepository(HttpClient httpClient, IConfiguration configuration, ILogger<GhtkRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _submitExpressOrderPath = configuration["GHTK:Endpoints:SubmitOrderExpress"] ?? "/services/shipment/order/?ver=1";
            _printLabelPathTemplate = configuration["GHTK:Endpoints:PrintLabel"] ?? "/services/label/{label}";
            _trackingStatusPathTemplate = configuration["GHTK:Endpoints:TrackingStatus"] ?? "/services/shipment/v2/{label}";
        }

        public async Task<GhtkFeeResponse> CalculateFeeAsync(GhtkFeeRequest request, CancellationToken cancellationToken = default)
        {
            var queryParams = new List<string>
            {
                $"pick_province={Uri.EscapeDataString(request.PickProvince ?? string.Empty)}",
                $"pick_district={Uri.EscapeDataString(request.PickDistrict ?? string.Empty)}",
                $"province={Uri.EscapeDataString(request.Province)}",
                $"district={Uri.EscapeDataString(request.District)}",
                $"weight={request.Weight}"
            };

            if (!string.IsNullOrWhiteSpace(request.PickWard))
            {
                queryParams.Add($"pick_ward={Uri.EscapeDataString(request.PickWard)}");
            }

            if (!string.IsNullOrWhiteSpace(request.PickAddress))
            {
                queryParams.Add($"pick_address={Uri.EscapeDataString(request.PickAddress)}");
            }

            if (!string.IsNullOrWhiteSpace(request.Ward))
            {
                queryParams.Add($"ward={Uri.EscapeDataString(request.Ward)}");
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                queryParams.Add($"address={Uri.EscapeDataString(request.Address)}");
            }

            if (request.Value.HasValue)
            {
                queryParams.Add($"value={request.Value.Value}");
            }

            if (!string.IsNullOrWhiteSpace(request.Transport))
            {
                queryParams.Add($"transport={Uri.EscapeDataString(request.Transport)}");
            }

            var url = $"/services/shipment/fee?{string.Join("&", queryParams)}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("GHTK fee response status: {StatusCode}, body: {Body}", response.StatusCode, body);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("GHTK authentication failed. Check Token and PartnerCode.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GHTK fee API failed: {(int)response.StatusCode} - {body}");
            }

            var result = JsonSerializer.Deserialize<GhtkFeeResponse>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new HttpRequestException("Failed to deserialize GHTK fee response.");
        }

        public async Task<GhtkSubmitOrderResponse> SubmitExpressOrderAsync(GhtkSubmitOrderRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonSerializer.Serialize(request);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, _submitExpressOrderPath)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("GHTK submit express order response status: {StatusCode}, body: {Body}", response.StatusCode, body);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("GHTK authentication failed. Check Token and PartnerCode.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GHTK submit express order API failed: {(int)response.StatusCode} - {body}");
            }

            var result = JsonSerializer.Deserialize<GhtkSubmitOrderResponse>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                throw new HttpRequestException("Failed to deserialize GHTK submit order response.");
            }

            if (!result.Success)
            {
                throw new Exception($"GHTK error: {result.Message}");
            }

            return result;
        }

        public async Task<GhtkPrintLabelFile> DownloadPrintLabelAsync(string label, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("Label is required.");
            }

            var path = _printLabelPathTemplate.Replace("{label}", Uri.EscapeDataString(label.Trim()));
            using var response = await _httpClient.GetAsync(path, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("GHTK authentication failed. Check Token and PartnerCode.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"GHTK print label API failed: {(int)response.StatusCode} - {errorBody}");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/pdf";

            _logger.LogInformation("GHTK print label response status: {StatusCode}, contentType: {ContentType}, size: {Size}",
                response.StatusCode, contentType, bytes.Length);

            return new GhtkPrintLabelFile
            {
                Content = bytes,
                ContentType = contentType,
                FileName = $"{label}.pdf"
            };
        }

        public async Task<GhtkTrackingStatusResponse> GetTrackingStatusAsync(string label, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("Label is required.");
            }

            var path = _trackingStatusPathTemplate.Replace("{label}", Uri.EscapeDataString(label.Trim()));
            using var response = await _httpClient.GetAsync(path, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("GHTK tracking response status: {StatusCode}, body: {Body}", response.StatusCode, body);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("GHTK authentication failed. Check Token and PartnerCode.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GHTK tracking API failed: {(int)response.StatusCode} - {body}");
            }

            var result = JsonSerializer.Deserialize<GhtkTrackingStatusResponse>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                throw new HttpRequestException("Failed to deserialize GHTK tracking response.");
            }

            if (!result.Success)
            {
                throw new Exception($"GHTK tracking error: {result.Message}");
            }

            return result;
        }
    }
}