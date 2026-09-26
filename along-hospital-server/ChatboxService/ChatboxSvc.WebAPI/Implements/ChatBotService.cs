using ChatboxSvc.WebAPI.Commons;
using ChatboxSvc.WebAPI.Interfaces;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.AppointmentEvents;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.WorkScheduleEvents;
using Microsoft.Extensions.AI;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Services.Interfaces;
using System.Text.Json;

namespace ChatboxSvc.WebAPI.Implements
{
    public class ChatBotService(
        IMessageBus messageBus,
        IChatClient chatClient,
        IEmbeddingService embeddingService,
        IVectorSearchService vectorSearchService,
        ICacheService cacheService) : IChatBotService
    {
        private const string FallbackResponse = "Sorry, I couldn't process your request at this time.";

        private readonly IMessageBus _messageBus = messageBus;
        private readonly IChatClient _chatClient = chatClient;
        private readonly IEmbeddingService _embeddingService = embeddingService;
        private readonly IVectorSearchService _vectorSearchService = vectorSearchService;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<string> GetBotResponseAsync(string userMessage, List<ChatMessage> chatHistory)
        {
            var fullPrompt = new List<ChatMessage>
            {
                new(ChatRole.System, GeneralLLMPrompt.ChatbotPrompt),
                new(ChatRole.System, await this.BuildContextAsync(userMessage))
            };

            fullPrompt.AddRange(chatHistory);
            fullPrompt.Add(new ChatMessage(ChatRole.User, userMessage));

            try
            {
                var chatResponse = await _chatClient.GetResponseAsync(fullPrompt);
                if (chatResponse == null || string.IsNullOrWhiteSpace(chatResponse.Text))
                {
                    throw new Exception();
                }

                return chatResponse.Text;
            }
            catch
            {
                return FallbackResponse;
            }
        }

        #region Context Building
        private async Task<string> BuildContextAsync(string userMessage)
        {
            var contextResult = "CONTEXT:\n";
            List<string> contextSections = [];
            contextSections.AddRange(await this.BuildCatalogSummaryContextAsync());
            contextSections.AddRange(await this.BuildVectorContextAsync(userMessage));
            contextSections.AddRange(await this.BuildOperatingHoursContextAsync());

            if (!contextSections.Any())
            {
                return contextResult + "(no data available)";
            }

            return contextResult + string.Join("\n", contextSections);
        }

        private async Task<List<string>> BuildVectorContextAsync(string userMessage)
        {
            try
            {
                var medicineVectorType = "medicine";
                var medicalServiceVectorType = "medical_service";
                var specialtyVectorType = "specialty";

                List<string> contextSections = [];
                var catalogData = await this.GetCatalogDataCachedAsync();

                if (!catalogData.Medicines.Any() &&
                    !catalogData.MedicalServices.Any() &&
                    !catalogData.Specialties.Any())
                {
                    return contextSections;
                }

                var queryVector = await _embeddingService.EmbedAsync(userMessage);
                Dictionary<string, float> medicineScores = queryVector.Length == 0
                    ? []
                    : await this.BuildScoreMapAsync<GetAllMedicinesContractItem>(
                        queryVector,
                        medicineVectorType,
                        Math.Max(catalogData.Medicines.Count, 1),
                        m => m.Name);

                Dictionary<string, float> medicalServiceScores = queryVector.Length == 0
                    ? []
                    : await this.BuildScoreMapAsync<GetAllMedicalServicesContractItem>(
                        queryVector,
                        medicalServiceVectorType,
                        Math.Max(catalogData.MedicalServices.Count, 1),
                        s => s.Name);

                Dictionary<string, float> specialtyScores = queryVector.Length == 0
                    ? []
                    : await this.BuildScoreMapAsync<GetAllSpecialtiesContractItem>(
                        queryVector,
                        specialtyVectorType,
                        Math.Max(catalogData.Specialties.Count, 1),
                        s => s.Name);

                var medicines = catalogData.Medicines
                    .OrderByDescending(m => this.GetScoreOrDefault(medicineScores, m.Name, -1f))
                    .ThenBy(m => m.Name)
                    .ToList();

                if (medicines.Any())
                {
                    contextSections.Add($"""
                        MEDICINES:
                        {string.Join("\n",
                            medicines.Select(m => $"- Name: {m.Name}, Brand: {m.Brand}, Category: {m.MedicineCategoryName}, Unit: {m.MedicineUnit}"))}
                        """);
                }

                var services = catalogData.MedicalServices
                    .OrderByDescending(s => this.GetScoreOrDefault(medicalServiceScores, s.Name, -1f))
                    .ThenBy(s => s.Name)
                    .ToList();

                if (services.Any())
                {
                    contextSections.Add($"""
                        MEDICAL SERVICES:
                        {string.Join("\n", services.Select(s => $"- Name: {s.Name}, Description: {s.Description}, Price: {s.Price} USD"))}
                        """);
                }

                var specialties = catalogData.Specialties
                    .OrderByDescending(s => this.GetScoreOrDefault(specialtyScores, s.Name, -1f))
                    .ThenBy(s => s.Name)
                    .ToList();

                if (specialties.Any())
                {
                    contextSections.Add($"""
                        SPECIALTIES:
                        {string.Join("\n", specialties.Select(s => $"- {s.Name}"))}
                        """);
                }

                return contextSections;
            }
            catch
            {
                return [];
            }
        }

        private async Task<List<string>> BuildOperatingHoursContextAsync()
        {
            try
            {
                List<string> contextSections = [];
                var catalogData = await this.GetCatalogDataCachedAsync();

                var workingShifts = catalogData.WorkingShifts
                    .OrderBy(shift => shift.StartTime)
                    .ThenBy(shift => shift.Name)
                    .ToList();

                if (workingShifts.Any())
                {
                    contextSections.Add($"""
                        WORKING SHIFTS (NON-OVERTIME):
                        {string.Join("\n", workingShifts.Select(shift => $"- {shift.Name}: {shift.StartTime:HH\\:mm} - {shift.EndTime:HH\\:mm}"))}
                        """);
                }

                var appointmentTimeSlots = catalogData.TimeSlots
                    .OrderBy(timeSlot => timeSlot.Time)
                    .ToList();

                if (appointmentTimeSlots.Any())
                {
                    contextSections.Add($"""
                        APPOINTMENT TIME SLOTS:
                        {string.Join("\n", appointmentTimeSlots.Select(timeSlot => $"- {timeSlot.Time:HH\\:mm} (Capacity per doctor: {timeSlot.CapacityPerDoctor})"))}
                        """);
                }

                return contextSections;
            }
            catch
            {
                return [];
            }
        }

        private async Task<Dictionary<string, float>> BuildScoreMapAsync<TItem>(
            float[] queryVector,
            string vectorType,
            int maxResults,
            Func<TItem, string?> keySelector)
        {
            var scoreMap = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

            var vectorResults = await _vectorSearchService.SearchAsync(queryVector, vectorType, maxResults);
            foreach (var vectorResult in vectorResults)
            {
                if (string.IsNullOrWhiteSpace(vectorResult.PayloadJson))
                {
                    continue;
                }

                var payload = JsonSerializer.Deserialize<TItem>(vectorResult.PayloadJson);
                if (payload == null)
                {
                    continue;
                }

                var key = keySelector(payload);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                if (!scoreMap.TryGetValue(key, out var existingScore) || vectorResult.Score > existingScore)
                {
                    scoreMap[key] = vectorResult.Score;
                }
            }

            return scoreMap;
        }

        private float GetScoreOrDefault(Dictionary<string, float> scoreMap, string? key, float defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            return scoreMap.TryGetValue(key, out var score)
                ? score
                : defaultValue;
        }

        private async Task<List<string>> BuildCatalogSummaryContextAsync()
        {
            try
            {
                var summary = await this.GetCatalogSummaryCachedAsync();
                return
                [
                    $"""
                    CATALOG TOTALS (SYSTEM-WIDE):
                    - Medicines total: {summary.MedicinesTotal}
                    - Medical services total: {summary.MedicalServicesTotal}
                    - Specialties total: {summary.SpecialtiesTotal}
                    - Working shifts total (non-overtime): {summary.WorkingShiftsTotal}
                    - Appointment time slots total: {summary.TimeSlotsTotal}
                    - Note: item lists in other sections include full catalog items. Clinical catalog lists are ordered by relevance score.
                    """
                ];
            }
            catch
            {
                return [];
            }
        }
        #endregion

        #region Cached Data Retrieval
        private async Task<CatalogSummary> GetCatalogSummaryCachedAsync()
        {
            var catalogData = await this.GetCatalogDataCachedAsync();

            return new CatalogSummary
            {
                MedicinesTotal = catalogData.Medicines.Count,
                MedicalServicesTotal = catalogData.MedicalServices.Count,
                SpecialtiesTotal = catalogData.Specialties.Count,
                WorkingShiftsTotal = catalogData.WorkingShifts.Count,
                TimeSlotsTotal = catalogData.TimeSlots.Count
            };
        }

        private async Task<CatalogData> GetCatalogDataCachedAsync()
        {
            const string dataCacheKey = "AlongHospital:Catalog:Data";
            const string cacheKey = "AlongHospital:Catalog:Summary";
            const int cacheDurationHours = 24;

            var cached = await _cacheService.GetAsync(dataCacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<CatalogData>(cached) ?? new CatalogData();
            }

            var medicinesContract = await _messageBus
                .RequestAsync<GetAllPublicMedicinesEvent, GetAllPublicMedicinesContract>(
                    new GetAllPublicMedicinesEvent());

            var medicalServicesContract = await _messageBus
                .RequestAsync<GetAllMedicalServicesEvent, GetAllMedicalServicesContract>(
                    new GetAllMedicalServicesEvent());

            var specialtiesContract = await _messageBus
                .RequestAsync<GetAllSpecialtiesEvent, GetAllSpecialtiesContract>(
                    new GetAllSpecialtiesEvent());

            var workingShiftsContract = await _messageBus
                .RequestAsync<GetAllWorkingShiftsEvent, GetAllWorkingShiftsContract>(
                    new GetAllWorkingShiftsEvent());

            var timeSlotsContract = await _messageBus
                .RequestAsync<GetAllTimeSlotsEvent, GetAllTimeSlotsContract>(
                    new GetAllTimeSlotsEvent());

            var catalogData = new CatalogData
            {
                Medicines = medicinesContract.Medicines,
                MedicalServices = medicalServicesContract.MedicalServices,
                Specialties = specialtiesContract.Specialties,
                WorkingShifts = workingShiftsContract.Data,
                TimeSlots = timeSlotsContract.Data
            };

            var summary = new CatalogSummary
            {
                MedicinesTotal = catalogData.Medicines.Count,
                MedicalServicesTotal = catalogData.MedicalServices.Count,
                SpecialtiesTotal = catalogData.Specialties.Count,
                WorkingShiftsTotal = catalogData.WorkingShifts.Count,
                TimeSlotsTotal = catalogData.TimeSlots.Count
            };

            await _cacheService.SetAsync(
                dataCacheKey,
                JsonSerializer.Serialize(catalogData),
                TimeSpan.FromHours(cacheDurationHours));

            await _cacheService.SetAsync(
                cacheKey,
                JsonSerializer.Serialize(summary),
                TimeSpan.FromHours(cacheDurationHours));

            return catalogData;
        }

        private sealed class CatalogData
        {
            public List<GetAllMedicinesContractItem> Medicines { get; init; } = [];
            public List<GetAllMedicalServicesContractItem> MedicalServices { get; init; } = [];
            public List<GetAllSpecialtiesContractItem> Specialties { get; init; } = [];
            public List<GetShiftContract> WorkingShifts { get; init; } = [];
            public List<GetTimeSlotContract> TimeSlots { get; init; } = [];
        }

        private sealed class CatalogSummary
        {
            public int MedicinesTotal { get; init; }
            public int MedicalServicesTotal { get; init; }
            public int SpecialtiesTotal { get; init; }
            public int WorkingShiftsTotal { get; init; }
            public int TimeSlotsTotal { get; init; }
        }
        #endregion
    }
}