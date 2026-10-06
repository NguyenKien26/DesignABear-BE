namespace DesignABear.Services.DTOs.Requests
{
    public class CreatePersonalizationRuleRequest
    {
        public Guid BaseProductId { get; set; }
        public Guid GroupId { get; set; }
        public Guid AllowedComponentProductId { get; set; }
        public bool IsRequired { get; set; }
        public int MaxQuantity { get; set; }
        public string RuleType { get; set; } = null!;
    }
}
