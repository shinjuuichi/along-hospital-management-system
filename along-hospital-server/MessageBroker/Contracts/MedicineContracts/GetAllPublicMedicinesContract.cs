using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicineContracts
{
    public record GetAllPublicMedicinesContract : BaseContract
    {
        public List<GetAllMedicinesContractItem> Medicines { get; init; } = [];
    }

    public record GetAllMedicinesContractItem
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Brand { get; init; }
        public string? MedicineUnit { get; init; }
        public string? MedicineCategoryName { get; init; }
    }
}