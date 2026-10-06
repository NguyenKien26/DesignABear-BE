namespace DesignABear.Services.DTOs.Responses
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPersonalizable { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; } = string.Empty;
        public float WeightGram { get; set; } = 500f;
        public decimal BaseCost { get; set; }
        public decimal AssemblyCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int Available => OnHand - Reserved;
        public string ProductType { get; set; } = "Standard";
        public string? ImageUrl { get; set; }
        public IEnumerable<ProductMediaResponse> Media { get; set; } = new List<ProductMediaResponse>();
    }

    public class ProductDetailResponse : ProductResponse
    {
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public IEnumerable<CharacterResponse> Characters { get; set; } = new List<CharacterResponse>();
        public string? Model3DUrl { get; set; }
        public IEnumerable<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();
        public IEnumerable<ProductComboImageResponse> ComboImages { get; set; } = new List<ProductComboImageResponse>();
        public IEnumerable<ProductVariantResponse> Variants { get; set; } = new List<ProductVariantResponse>();
        public IEnumerable<AccessoryResponse> Accessories { get; set; } = new List<AccessoryResponse>();
    }
}
