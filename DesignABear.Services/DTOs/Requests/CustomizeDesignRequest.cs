namespace DesignABear.Services.DTOs.Requests
{
    public class CustomizeDesignRequest
    {
        public Guid BaseProductId { get; set; }
        public List<Guid> SelectedRuleIds { get; set; } = new List<Guid>();
    }
}
