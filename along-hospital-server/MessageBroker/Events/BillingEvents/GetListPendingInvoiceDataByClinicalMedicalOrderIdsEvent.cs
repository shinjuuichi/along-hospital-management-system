using MessageBroker.Abstractions;
using MessageBroker.Contracts.MedicalOrderContracts;

namespace MessageBroker.Events.BillingEvents
{
    public record GetListPendingInvoiceDataByClinicalMedicalOrderIdsEvent : BaseEvent
    {
        public Dictionary<string, GetClinicalMedicalOrderContract> ClinicalMedicalOrderDict { get; init; } = [];
    }
}