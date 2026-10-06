using System;
using System.Collections.Generic;

namespace DesignABear.Services.DTOs.Responses
{
    public class CharacterWithAccessoriesResponse
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? LicenseBrand { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public List<ProductWithAccessoriesResponse> Products { get; set; } = new();
    }

    public class ProductWithAccessoriesResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public List<PersonalizationRuleResponse> Accessories { get; set; } = new();
    }
}
