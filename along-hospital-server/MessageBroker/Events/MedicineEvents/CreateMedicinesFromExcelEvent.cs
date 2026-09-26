using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record CreateMedicinesFromExcelEvent : BaseEvent
    {
        public List<CreateMedicineFromExcelEventItem> CreateMedicineFromExcelEventItems { get; init; } = [];
    }

    public record CreateMedicineFromExcelEventItem
    {
        public string? Name { get; init; }
        public double Price { get; init; }
    }
}