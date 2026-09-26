using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendLowStockMedicineEmailEvent : BaseEvent
    {
        public int MedicineId { get; init; }
        public string? Name { get; init; }
        public string? Brand { get; init; }
        public string? MedicineUnit { get; init; }
        public string[] Images { get; init; } = [];
        public string? CategoryName { get; init; }
        public int Quantity { get; init; }
        public int? MinQuantity { get; init; }
        public DateTime? LastImportDate { get; init; }
    }

    public record SendLowStockMedicinesEmailEvent : BaseEvent
    {
        public List<SendLowStockMedicineEmailEvent> Data { get; init; } = [];
    }
}
