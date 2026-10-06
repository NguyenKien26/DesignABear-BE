using System.Text.Json.Serialization;

namespace DesignABear.Repositories.Integrations.Ghtk.Models
{
    public class GhtkSubmitOrderRequest
    {
        [JsonPropertyName("products")]
        public List<GhtkSubmitOrderProduct> Products { get; set; } = new();

        [JsonPropertyName("order")]
        public GhtkSubmitOrderData Order { get; set; } = new();
    }

    public class GhtkSubmitOrderData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pick_name")]
        public string PickName { get; set; } = string.Empty;

        [JsonPropertyName("pick_address")]
        public string PickAddress { get; set; } = string.Empty;

        [JsonPropertyName("pick_province")]
        public string PickProvince { get; set; } = string.Empty;

        [JsonPropertyName("pick_district")]
        public string PickDistrict { get; set; } = string.Empty;

        [JsonPropertyName("pick_ward")]
        public string? PickWard { get; set; }

        [JsonPropertyName("pick_tel")]
        public string PickTel { get; set; } = string.Empty;

        [JsonPropertyName("tel")]
        public string Tel { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("province")]
        public string Province { get; set; } = string.Empty;

        [JsonPropertyName("district")]
        public string District { get; set; } = string.Empty;

        [JsonPropertyName("ward")]
        public string? Ward { get; set; }

        [JsonPropertyName("hamlet")]
        public string? Hamlet { get; set; }

        [JsonPropertyName("is_freeship")]
        public int? IsFreeShip { get; set; }

        [JsonPropertyName("pick_money")]
        public int PickMoney { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }

        [JsonPropertyName("transport")]
        public string? Transport { get; set; }

        [JsonPropertyName("pick_option")]
        public string? PickOption { get; set; }

        [JsonPropertyName("deliver_option")]
        public string? DeliverOption { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("weight_option")]
        public string? WeightOption { get; set; }

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
    }
    public class GhtkSubmitOrderProduct
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("product_code")]
        public string? ProductCode { get; set; }
    }
}