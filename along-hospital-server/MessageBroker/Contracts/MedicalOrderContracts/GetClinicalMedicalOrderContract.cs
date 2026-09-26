namespace MessageBroker.Contracts.MedicalOrderContracts
{
    public record GetClinicalMedicalOrderContract : GetMedicalOrderContract
    {
        public string? ClinicalMedicalOrderStatus { get; init; }

        public int PendingInvoiceId { get; init; }

        public List<GetClinicalMedicalOrderDetailContract> ClinicalMedicalOrderDetails { get; init; } = [];
    }

    public record GetClinicalMedicalOrderDetailContract
    {
        public string? Id { get; init; }

        public int Quantity { get; init; }

        public string? ClinicalMedicalOrderDetailStatus { get; init; }

        public int MedicalServiceId { get; init; }

        public GetMedicalServiceSnapshotContract? MedicalServiceSnapshot { get; init; }

        public record GetMedicalServiceSnapshotContract
        {
            public string? Name { get; init; }

            public string? Description { get; init; }

            public string? Code { get; init; }
        }
    }
}
