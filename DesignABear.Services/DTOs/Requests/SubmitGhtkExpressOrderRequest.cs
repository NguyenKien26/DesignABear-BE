using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class SubmitGhtkExpressOrderRequest
    {
        [Required]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        public string CustomerAddress { get; set; } = string.Empty;

        [Required]
        public string CustomerProvince { get; set; } = string.Empty;

        [Required]
        public string CustomerDistrict { get; set; } = string.Empty;

        public string? CustomerWard { get; set; }
        public string? Hamlet { get; set; }

        [Range(0, int.MaxValue)]
        public int PickMoney { get; set; }

        [Range(0, int.MaxValue)]
        public int? Value { get; set; }

        public string? Transport { get; set; }
        public string? Note { get; set; }
        public int? IsFreeShip { get; set; }
        public string? PickOption { get; set; }
        public string? DeliverOption { get; set; }
        public string? WeightOption { get; set; }

        [MinLength(1)]
        public List<SubmitGhtkExpressOrderProductRequest> Products { get; set; } = new();
    }

    public class SubmitGhtkExpressOrderProductRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Weight { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string? ProductCode { get; set; }
    }
}