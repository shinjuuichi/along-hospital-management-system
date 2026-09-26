using MessageBroker.Abstractions;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;

namespace MessageBroker.Contracts.StaffContracts
{
    public record GetStaffGroupByIdContract : BaseContract
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public List<StaffGroupMemberContractItem> StaffGroupMembers { get; init; } = [];

        public record StaffGroupMemberContractItem
        {
            public int StaffGroupId { get; init; }

            public int StaffId { get; init; }

            public GetStaffDataByUserIdContract? Staff { get; init; }
        }
    }

    public record GetListStaffGroupDataByIdsContract : BaseContract
    {
        public List<GetStaffGroupByIdContract> Data { get; init; } = [];
    }
}