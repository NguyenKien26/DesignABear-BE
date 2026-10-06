namespace DesignABear.Repositories.Models
{
    public static class ProductionStatus
    {
        public const string InQueue = "IN_QUEUE";
        public const string Assigned = "ASSIGNED";
        public const string InProgress = "IN_PROGRESS";
        public const string Completed = "COMPLETED";
        public const string AwaitingQC = "AWAITING_QC";
        public const string UnderQC = "UNDER_QC";
        public const string ReworkRequired = "REWORK_REQUIRED";
        public const string ReworkPending = "REWORK_PENDING";
        public const string ReworkInProgress = "REWORK_IN_PROGRESS";
        public const string Designing = "DESIGNING";
        public const string Finished = "FINISHED";
    }
}
