using System.ComponentModel.DataAnnotations;

namespace DesignABear.Services.DTOs.Requests
{
    public class UpdateCategoryRequest
    {
        public Guid? ParentId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(160)]
        public string Slug { get; set; } = null!;
    }
}
