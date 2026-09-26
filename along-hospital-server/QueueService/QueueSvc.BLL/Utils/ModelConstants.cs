namespace QueueSvc.BLL.Utils
{
    public static class ModelConstants
    {
        public const string MEDICAL_HISTORY_STATUS_PENDING_PAYMENT = "PendingPayment";
        public const string MEDICAL_HISTORY_STATUS_DRAFT = "Draft";
        public const string MEDICAL_HISTORY_STATUS_COMPLETED = "Completed";
        public const string MEDICAL_HISTORY_STATUS_CANCELLED = "Cancelled";

        public const string MEDICAL_HISTORY_TYPE_OUTPATIENT = "Outpatient";
        public const string MEDICAL_HISTORY_TYPE_INPATIENT = "Inpatient";

        public const string APPOINTMENT_STATUS_SCHEDULED = "Scheduled";
        public const string APPOINTMENT_STATUS_COMPLETED = "Completed";
        public const string APPOINTMENT_PAYMENT_STATUS_COMPLETED = "Completed";
        public const string APPOINTMENT_MEETING_TYPE_IN_PERSON = "InPerson";
    }
}
