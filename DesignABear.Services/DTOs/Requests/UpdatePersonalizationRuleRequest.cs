namespace DesignABear.Services.DTOs.Requests
{
    public class UpdatePersonalizationRuleRequest
    {
        public bool IsRequired { get; set; }
        public int MaxQuantity { get; set; }
        public string RuleType { get; set; } = null!;
    }
}
