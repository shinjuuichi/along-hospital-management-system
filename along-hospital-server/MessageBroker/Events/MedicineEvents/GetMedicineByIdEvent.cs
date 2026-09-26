using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record GetMedicineByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListMedicineDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }

    public record GetListMedicineDataByNamesEvent : BaseEvent
    {
        public List<string> Names { get; init; } = [];
    }

    public record GetListMedicineDataBySKUCodesEvent : BaseEvent
    {
        public List<string> SKUCodes { get; init; } = [];
    }
}