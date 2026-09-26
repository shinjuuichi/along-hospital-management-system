using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicineContracts
{
    public record GetMedicineByIdContract : BaseContract
    {
        public int Id { get; init; }
        public string? SKUCode { get; init; }
        public string? Name { get; init; }
        public string? Brand { get; init; }
        public string? MedicineUnit { get; init; }
        public double Price { get; init; }
        public string[] MedicineImages { get; init; } = [];
        public int CategoryId { get; init; }
        public string? CategoryName { get; init; }
        public bool IsPublic { get; init; }
    }

    public record GetListMedicineDataByIdsContract : BaseContract
    {
        public List<GetMedicineByIdContract> Data { get; init; } = [];
    }

    public record GetListMedicineDataByNamesContract : BaseContract
    {
        public List<GetMedicineByIdContract> Data { get; init; } = [];
    }
}
