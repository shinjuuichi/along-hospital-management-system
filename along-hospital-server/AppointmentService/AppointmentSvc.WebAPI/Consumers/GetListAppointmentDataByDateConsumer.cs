using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetListAppointmentDataByDateConsumer(
        IAppointmentQueryService appointmentQueryService,
        IMapper mapper)
        : RequestConsumer<GetListAppointmentDataByDateEvent, GetListAppointmentDataContract>
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListAppointmentDataContract> Handle(ConsumeContext<GetListAppointmentDataByDateEvent> context)
        {
            var date = context.Message.Date;

            var appointmentDTOs = await _appointmentQueryService.GetAllByDateAsync(date, ignoreRequestingValue: true);
            var appointmentContracts = _mapper.Map<List<GetAppointmentContract>>(appointmentDTOs);

            return new() { Data = appointmentContracts };
        }
    }
}
