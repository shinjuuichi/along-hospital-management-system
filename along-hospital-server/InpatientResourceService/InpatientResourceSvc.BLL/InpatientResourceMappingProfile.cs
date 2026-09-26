using InpatientResourceSvc.BLL.DTOs.BedDTOs;
using InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs;
using InpatientResourceSvc.BLL.DTOs.RoomDTOs;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace InpatientResourceSvc.BLL
{
    public class InpatientResourceMappingProfile : BaseMappingProfile
    {
        public InpatientResourceMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MapContract();
        }

        private void MapContract()
        {
            CreateMap<GetSpecialtyByIdContract, GetRoomDTO>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.SpecialtyName, opt => opt.MapFrom(s => s.Name));
            CreateMap<GetBedOccupancyDTO, GetBedOccupancyByMedicalHistoryIdContract>();
            CreateMap<GetBedOccupancyDetailDTO, GetBedContract>();
            CreateMap<GetBedDTO, GetBedContract>();
            CreateMap<GetRoomDTO, GetRoomContract>();
        }
    }
}
