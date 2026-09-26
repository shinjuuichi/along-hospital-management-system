using MedicalServiceSvc.BLL.DTOs;
using MedicalServiceSvc.DAL.Models;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace MedicalServiceSvc.BLL
{
    public class MedicalServiceMappingProfile : BaseMappingProfile
    {
        public MedicalServiceMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<GetMedicalServiceDTO, GetMedicalServiceContract>();
            CreateMap<GetMedicalServiceDTO, GetAllMedicalServicesContractItem>();

            CreateMap<CreateMedicalServiceEvent, UpsertMedicalServiceDTO>();
            CreateMap<GetMedicalServiceDTO, CreateMedicalServiceContract>();
            CreateMap<MedicalService, GetMedicalServiceDTO>()
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.MedicalServiceRoles.Select(r => r.Role.ToString())));
        }
    }
}