using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicineContracts
{
    public record CreateMedicinesFromExcelContract : BaseContract
    {
        public List<CreateMedicineDataContractItem> Data { get; init; } = [];
    }

    public record CreateMedicineDataContractItem
    {
        public int MedicineId { get; init; }
        public string? Name { get; init; }
        public double Price { get; init; }
    }
}