using System;

namespace DesignABear.Services.DTOs.Responses
{
    public class FavoriteResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImageUrl { get; set; }
    }
}
