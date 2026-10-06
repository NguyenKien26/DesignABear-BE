using System.Collections.Generic;

namespace DesignABear.Services.DTOs.Responses
{
    public class ProductFullDetailResponse : ProductDetailResponse
    {
        public IEnumerable<InventoryResponse> AllInventories { get; set; } = new List<InventoryResponse>();
        public IEnumerable<CollectionResponse> Collections { get; set; } = new List<CollectionResponse>();
        public IEnumerable<PersonalizationRuleResponse> BasePersonalizationRules { get; set; } = new List<PersonalizationRuleResponse>();
        public IEnumerable<PersonalizationRuleResponse> AddonPersonalizationRules { get; set; } = new List<PersonalizationRuleResponse>();
        
        // Stats
        public int ViewCount { get; set; }
        public float DiscountRate { get; set; }
    }
}
