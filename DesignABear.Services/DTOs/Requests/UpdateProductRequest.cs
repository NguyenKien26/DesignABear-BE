using System;
using System.Collections.Generic;

namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Model3DUrl { get; set; }
        public bool IsPersonalizable { get; set; }
        public bool IsActive { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
        public List<Guid> CharacterIds { get; set; } = new List<Guid>();
        public List<Guid> AccessoryIds { get; set; } = new List<Guid>();
        public List<ProductVariantInlineRequest> Variants { get; set; } = new List<ProductVariantInlineRequest>();
        public List<CreateProductMediaInlineRequest> Media { get; set; } = new List<CreateProductMediaInlineRequest>();
        public List<ComboImageInlineRequest> ComboImages { get; set; } = new List<ComboImageInlineRequest>();
    }
}
