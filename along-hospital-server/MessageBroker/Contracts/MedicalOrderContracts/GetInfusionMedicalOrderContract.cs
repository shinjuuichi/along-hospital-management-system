namespace MessageBroker.Contracts.MedicalOrderContracts
{
    public record GetInfusionMedicalOrderContract : GetMedicalOrderContract
    {
        public List<GetInfusionMedicalOrderDetailContract> InfusionMedicalOrderDetails { get; init; } = [];
    }

    public record GetInfusionMedicalOrderDetailContract
    {
        public string? Id { get; init; }

        public string? Rate { get; init; }

        public string? Frequency { get; init; }

        public string? Duration { get; init; }

        public string? InfusionMedicalOrderDetailExecutionStatus { get; init; }

        public string? Note { get; init; }

        public int MedicineId { get; init; }

        public GetMedicineSnapshotContract? MedicineSnapshot { get; init; }

        public record GetMedicineSnapshotContract
        {
            public string? Name { get; init; }

            public string? Brand { get; init; }

            public string? MedicineUnit { get; init; }

            public string? MedicineImage { get; init; }

            public string? CategoryName { get; init; }
        }
    }
}
