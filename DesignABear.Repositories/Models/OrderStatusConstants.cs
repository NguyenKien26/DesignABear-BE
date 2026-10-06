namespace DesignABear.Repositories.Models
{
    public static class OrderStatusConstants
    {
        public const string Pending = "PENDING";
        public const string Cancelled = "CANCELLED";
        public const string Rejected = "REJECTED";
        public const string PendingPayment = "PENDING_PAYMENT";
        public const string Completed = "COMPLETED";
        public const string Processing = "PROCESSING";
        public const string Shipping = "SHIPPING";
        public const string Delivered = "DELIVERED";
        public const string Refunded = "REFUNDED";
        public const string Paid = "PAID";
        public const string Printing = "PRINTING";
        public const string ReadyForPickup = "READY_FOR_PICKUP";
    }

    public static class PaymentStatusConstants
    {
        public const string Pending = "PENDING";
        public const string Captured = "CAPTURED";
        public const string Failed = "FAILED";
        public const string Cancelled = "CANCELLED";
        public const string Refunded = "REFUNDED";
    }

    public static class ProductionJobStatusConstants
    {
        public const string InQueue = "IN_QUEUE";
        public const string Processing = "PROCESSING";
        public const string Finished = "FINISHED";
        public const string PendingPayment = "PENDING_PAYMENT";
        public const string Printing = "PRINTING";
        public const string QualityCheck = "QC_PENDING";
    }

    public static class PriorityConstants
    {
        public const int Low = 1;
        public const int Normal = 3;
        public const int Urgent = 5;
    }
}
