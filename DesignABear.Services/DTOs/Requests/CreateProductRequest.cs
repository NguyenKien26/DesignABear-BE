using System;
using System.Collections.Generic;

namespace DesignABear.Services.DTOs.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model3DUrl { get; set; }
        public bool IsPersonalizable { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string Sku { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
        public List<Guid> CharacterIds { get; set; } = new List<Guid>();
        public List<Guid> AccessoryIds { get; set; } = new List<Guid>();

         public List<ProductVariantInlineRequest> Variants { get; set; } = new List<ProductVariantInlineRequest>();

        public List<CreateProductMediaInlineRequest> Media { get; set; } = new List<CreateProductMediaInlineRequest>();

        public List<ComboImageInlineRequest> ComboImages { get; set; } = new List<ComboImageInlineRequest>();
    }

    public class CreateProductMediaInlineRequest
    {
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int SortOrder { get; set; } = 0;
    }

    public class ComboImageInlineRequest
    {
        public string CombinationKey { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }

    public class ProductVariantInlineRequest
    {
        public string SizeTag { get; set; } = null!;
        public string SizeDescription { get; set; } = string.Empty;
        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public float WeightGram { get; set; }
        public string? Sku { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; } 
    }
}
