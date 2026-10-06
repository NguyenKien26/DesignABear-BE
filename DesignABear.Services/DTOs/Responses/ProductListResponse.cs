using System;
using System.Collections.Generic;

namespace DesignABear.Services.DTOs.Responses
{
    public class ProductListResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public string? ShortDescription { get; set; }
        public int TotalSales { get; set; }
        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public decimal MinPrice { get; set; }
        public long ViewCountIn10Min { get; set; }
        public double DiscountRate { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int Available => OnHand - Reserved;
        public string ProductType { get; set; } = "Standard";
        public IEnumerable<ProductMediaResponse> Media { get; set; } = new List<ProductMediaResponse>();
        public IEnumerable<ProductVariantResponse> Variants { get; set; } = new List<ProductVariantResponse>();
    }
}
