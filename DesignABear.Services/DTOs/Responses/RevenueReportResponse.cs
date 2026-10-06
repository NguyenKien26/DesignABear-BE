namespace DesignABear.Services.DTOs.Responses
{
    public class RevenueReportResponse
    {
        public decimal TotalRevenue { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal TotalProductCost { get; set; }
        public decimal TotalEmployeeCost { get; set; }
        public decimal TotalRefund { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal ProfitMarginPercent { get; set; }

        public int TotalOrders { get; set; }

        public List<DailyRevenue> DailyBreakdown { get; set; } = new();
    }

    public class DailyRevenue
    {
        public DateTime Date { get; set; }

        public decimal Revenue { get; set; }
        public decimal Refund { get; set; }
        public decimal NetRevenue { get; set; }

        public decimal ProductCost { get; set; }
        public decimal EmployeeCost { get; set; }

        public decimal Profit { get; set; }
    }
}
