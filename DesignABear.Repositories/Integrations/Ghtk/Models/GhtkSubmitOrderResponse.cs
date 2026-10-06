using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkSubmitOrderResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public GhtkSubmittedOrderData? Order { get; set; }

        [JsonPropertyName("log_id")]
        public string? LogId { get; set; }

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }

    public class GhtkSubmittedOrderData
    {
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("partner_id")]
        public string? PartnerId { get; set; }

        [JsonPropertyName("fee")]
        public int? Fee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public int? InsuranceFee { get; set; }

        [JsonPropertyName("estimated_pick_time")]
        public string? EstimatedPickTime { get; set; }

        [JsonPropertyName("estimated_deliver_time")]
        public string? EstimatedDeliverTime { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }
}