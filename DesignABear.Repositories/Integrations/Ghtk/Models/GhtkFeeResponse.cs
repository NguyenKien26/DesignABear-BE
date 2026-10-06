using System.Text.Json.Serialization;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkFeeResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("fee")]
        public GhtkFeeData? Fee { get; set; }

        [JsonPropertyName("log_id")]
        public string? LogId { get; set; }

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }
    }

    public class GhtkFeeData
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("fee")]
        public int Fee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public int InsuranceFee { get; set; }

        [JsonPropertyName("delivery")]
        public bool Delivery { get; set; }

        [JsonPropertyName("include_vat")]
        public int? IncludeVat { get; set; }

        [JsonPropertyName("cost_id")]
        [JsonConverter(typeof(FlexibleStringJsonConverter))]
        public string? CostId { get; set; }
    }
}