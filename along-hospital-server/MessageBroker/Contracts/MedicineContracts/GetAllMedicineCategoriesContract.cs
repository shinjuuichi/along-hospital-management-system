using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicineContracts
{
    public record GetAllMedicineCategoriesContract : BaseContract
    {
        public List<GetAllMedicineCategoriesContractItem> MedicineCategories { get; init; } = [];
    }

    public record GetAllMedicineCategoriesContractItem
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }
}