using AutoMapper;
using MassTransit;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class CreateMedicalHistoryFromAppointmentConsumer(
        IMedicalHistoryCommandService medicalHistoryCommandService,
        IMapper mapper)
            : RequestConsumer<CreateMedicalHistoryFromAppointmentEvent, CreateMedicalHistoryFromAppointmentContract>
    {
        private readonly IMedicalHistoryCommandService _medicalHistoryCommandService = medicalHistoryCommandService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreateMedicalHistoryFromAppointmentContract> Handle(ConsumeContext<CreateMedicalHistoryFromAppointmentEvent> context)
        {
            var createMedicalHistoryDTO = _mapper.Map<CreateMedicalHistoryDTO>(context.Message);
            createMedicalHistoryDTO.MedicalHistoryType = nameof(MedicalHistoryTypeEnum.Outpatient);
            createMedicalHistoryDTO.IsCreatedFromAppointment = true;

            var getMedicalHistoryDTO = await _medicalHistoryCommandService.CreateAsync(createMedicalHistoryDTO);

            return new()
            {
                MedicalHistoryId = getMedicalHistoryDTO.Id
            };
        }
    }
}
