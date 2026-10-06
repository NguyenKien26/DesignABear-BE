using System;

namespace DesignABear.Services.DTOs.Responses
{
    public class PersonalizationRuleResponse
    {
        public Guid RuleId { get; set; }
        public Guid BaseProductId { get; set; }
        public Guid AddonProductId { get; set; }
        public string? AddonProductName { get; set; }
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }
        public string RuleType { get; set; } = "Optional";
        public int MaxQty { get; set; } = 1;
        public Guid? AllowedComponentProductId { get; set; } // Legacy/Alias for AddonProductId
        public ProductResponse? AddonProduct { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
