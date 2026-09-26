using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.DTOs.StatisticsDTOs;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace InventorySvc.BLL
{
    public class InventoryMappingProfile : BaseMappingProfile
    {
        public InventoryMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<CreateInventoryDTO, CreateInventoryContract>();
            CreateMap<CreateInventoryEvent, CreateInventoryDTO>();
            CreateMap<UpdateInventoryEvent, UpdateInventoryDTO>();
            CreateMap<AddQuantityFromImportEvent, UpdateInventoryDTO>();
            CreateMap<GetInventoryDTO, UpdateInventoryDTO>();
            CreateMap<GetInventoryDTO, CreateInventoryContract>();
            CreateMap<GetInventoryDTO, GetInventoryBySKUCodeContract>();
            CreateMap<GetMedicineByIdContract, GetMedicineDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.MedicineImages));

            CreateMap<GetMedicineSKUContract, GetMedicineDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MedicineId))
                .ForMember(dest => dest.SKUCode, opt => opt.MapFrom(src => src.SKUCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MedicineName))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.MedicineBrand))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.MedicineImages));

            //Map DTO to Event for Low Stock Email
            CreateMap<GetInventoryDTO, SendLowStockMedicineEmailEvent>()
                .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.Medicine!.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Medicine!.Name))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Medicine!.Brand))
                .ForMember(dest => dest.MedicineUnit, opt => opt.MapFrom(src => src.Medicine!.MedicineUnit))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Medicine!.Images))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Medicine!.CategoryName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.MinQuantity, opt => opt.MapFrom(src => src.MinQuantity))
                .ForMember(dest => dest.LastImportDate, opt => opt.MapFrom(src => src.LastImportDate));

            CreateMap<InventoryStatisticsDTO, GetInventoryStatisticsByDateRangeContract>();
        }
    }
}