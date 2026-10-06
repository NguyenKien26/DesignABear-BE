using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkTrackingStatusResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public GhtkTrackingOrderData? Order { get; set; }

        [JsonPropertyName("log_id")]
        public string? LogId { get; set; }

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }

    public class GhtkTrackingOrderData
    {
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("partner_id")]
        public string? PartnerId { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(FlexibleStringJsonConverter))]
        public string? Status { get; set; }

        [JsonPropertyName("status_id")]
        [JsonConverter(typeof(FlexibleStringJsonConverter))]
        public string? StatusId { get; set; }

        [JsonPropertyName("status_text")]
        public string? StatusText { get; set; }

        [JsonPropertyName("created")]
        public string? Created { get; set; }

        [JsonPropertyName("pickup_date")]
        public string? PickupDate { get; set; }

        [JsonPropertyName("deliver_date")]
        public string? DeliverDate { get; set; }

        [JsonPropertyName("customer_fullname")]
        public string? CustomerFullName { get; set; }

        [JsonPropertyName("customer_tel")]
        public string? CustomerTel { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("province")]
        public string? Province { get; set; }

        [JsonPropertyName("district")]
        public string? District { get; set; }

        [JsonPropertyName("ward")]
        public string? Ward { get; set; }

        [JsonPropertyName("ship_money")]
        public int? ShipMoney { get; set; }

        [JsonPropertyName("pick_money")]
        public int? PickMoney { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
    }
}