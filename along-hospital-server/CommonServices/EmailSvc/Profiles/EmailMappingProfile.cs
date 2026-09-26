using EmailSvc.DTOs;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace EmailSvc.Profiles
{
    public class EmailMappingProfile : BaseMappingProfile
    {
        public EmailMappingProfile()
             : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<SendOtpEmailEvent, SendOtpEmailDTO>();
            CreateMap<SendAppointmentReminderEmailEvent, SendAppointmentReminderEmailDTO>();
            CreateMap<SendVerificationLinkEmailEvent, SendLinkEmailDTO>();
            CreateMap<SendLowStockMedicineEmailEvent, SendLowStockMedicineDTO>();
      
            CreateMap<SendFeedbackRespondEvent, SendFeedbackRespondEmailDTO>();
      
            CreateMap<SendInterviewEmailEvent, SendInterviewEmailDTO>();
            CreateMap<SendInterviewResultEmailEvent, SendInterviewResultEmailDTO>();
      
            CreateMap<SendContractExpiringEmailEvent, SendContractExpiringEmailDTO>();
            CreateMap<SendCertificateExpirationReminderEmailEvent, SendCertificateExpirationReminderEmailDTO>();
      
            CreateMap<SendInvoiceEmailEvent, SendInvoiceEmailDTO>();
            CreateMap<SendInvoiceEmailLineItemEvent, SendInvoiceEmailLineItemDTO>();
        }
    }
}