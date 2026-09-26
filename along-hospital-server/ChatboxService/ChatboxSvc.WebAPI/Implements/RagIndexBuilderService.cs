using ChatboxSvc.WebAPI.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using Qdrant.Client;
using Qdrant.Client.Grpc;

using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using System.Text.Json;

namespace ChatboxSvc.WebAPI.Implements
{
    public class RagIndexBuilderService(
        IMessageBus messageBus,
        IEmbeddingService embeddingService,
        IVectorSearchService vectorSearchService,
        QdrantClient qdrantClient,
        AppConfiguration configuration) : IRagIndexBuilderService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IEmbeddingService _embeddingService = embeddingService;
        private readonly IVectorSearchService _vectorSearchService = vectorSearchService;
        private readonly QdrantClient _qdrantClient = qdrantClient;

        private readonly string CollectionName = configuration.QdrantConfig.CollectionName;
        private readonly int VectorSize = configuration.QdrantConfig.VectorSize;

        public async Task BuildAllAsync()
        {
            await this.EnsureQdrantCollectionCreatedAsync();

            await this.BuildMedicinesAsync();
            await this.BuildMedicalServicesAsync();
            await this.BuildSpecialtiesAsync();
        }

        private async Task EnsureQdrantCollectionCreatedAsync()
        {
            var exists = await _qdrantClient.CollectionExistsAsync(CollectionName);
            if (!exists)
            {
                await _qdrantClient.CreateCollectionAsync(
                    collectionName: CollectionName,
                    vectorsConfig: new VectorParams
                    {
                        Size = (ulong)VectorSize,
                        Distance = Distance.Cosine
                    });
            }
        }

        private async Task BuildMedicinesAsync()
        {
            try
            {
                var contract = await _messageBus.RequestAsync<GetAllPublicMedicinesEvent, GetAllPublicMedicinesContract>(
                    new GetAllPublicMedicinesEvent());

                foreach (var m in contract.Medicines)
                {
                    var doc = $"""
                    type: medicine
                    name: {m.Name}
                    brand: {m.Brand}
                    category: {m.MedicineCategoryName}
                    unit: {m.MedicineUnit}
                    vietnamese: thuốc, giảm đau, hạ sốt, kháng sinh, ho, cảm
                    """;

                    var vector = await _embeddingService.EmbedAsync(doc);

                    var payloadJson = JsonSerializer.Serialize(m);

                    await _vectorSearchService.UpsertAsync(
                        id: $"medicine-{m.Id}",
                        vector: vector,
                        type: "medicine",
                        payloadJson: payloadJson);
                }
            }
            catch { }
        }

        private async Task BuildMedicalServicesAsync()
        {
            try
            {
                var contract = await _messageBus.RequestAsync<GetAllMedicalServicesEvent, GetAllMedicalServicesContract>(
                    new GetAllMedicalServicesEvent());

                foreach (var s in contract.MedicalServices)
                {
                    var doc = $"""
                    type: medical_service
                    name: {s.Name}
                    description: {s.Description}
                    price: {s.Price}
                    vietnamese: khám tổng quát, xét nghiệm, siêu âm, chụp x quang
                    """;

                    var vector = await _embeddingService.EmbedAsync(doc);

                    var payloadJson = JsonSerializer.Serialize(s);

                    await _vectorSearchService.UpsertAsync(
                        id: $"medical-service-{s.Id}",
                        vector: vector,
                        type: "medical_service",
                        payloadJson: payloadJson);
                }
            }
            catch { }
        }

        private async Task BuildSpecialtiesAsync()
        {
            try
            {
                var contract = await _messageBus.RequestAsync<GetAllSpecialtiesEvent, GetAllSpecialtiesContract>(
                    new GetAllSpecialtiesEvent());

                foreach (var sp in contract.Specialties)
                {
                    var doc = $"""
                    type: specialty
                    name: {sp.Name}
                    vietnamese: chuyên khoa
                    """;

                    var vector = await _embeddingService.EmbedAsync(doc);

                    var payloadJson = JsonSerializer.Serialize(sp);

                    await _vectorSearchService.UpsertAsync(
                        id: $"specialty-{sp.Id}",
                        vector: vector,
                        type: "specialty",
                        payloadJson: payloadJson);
                }
            }
            catch { }
        }
    }
}
