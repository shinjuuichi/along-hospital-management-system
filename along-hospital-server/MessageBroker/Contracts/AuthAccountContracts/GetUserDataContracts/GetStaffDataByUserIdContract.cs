using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts
{
    public record GetStaffDataByUserIdContract : GetUserDataByUserIdContract
    {
        public int QualificationId { get; init; }

        public string? QualificationName { get; init; }

        public int SpecialtyId { get; init; }

        public string? SpecialtyName { get; init; }

        public string? BankCode { get; init; }

        public string? AccountNumber { get; init; }

        public int DependentQuantity { get; init; }

        public string? Status { get; init; }
    }

    public record GetListStaffDataByUserIdsContract : BaseContract
    {
        public List<GetStaffDataByUserIdContract> Data { get; init; } = [];
    }
}