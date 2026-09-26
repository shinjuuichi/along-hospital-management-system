using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.SupplierContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Mappers;
using SupplierSvc.BLL.DTOs.ImportDTOs;
using SupplierSvc.BLL.DTOs.ImportExcelDTOs;
using SupplierSvc.BLL.DTOs.ImportRequestDTOs;
using SupplierSvc.BLL.DTOs.StatisticsDTOs;
using SupplierSvc.BLL.DTOs.SupplierDTOs;
using SupplierSvc.DAL.Models;
using System.Reflection;

namespace SupplierSvc.BLL
{
    public class ImportMappingProfile : BaseMappingProfile
    {
        public ImportMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            // Medicine SKU mappings
            CreateMap<GetMedicineByIdContract, GetImportDetailDTO>();
            CreateMap<GetMedicineByIdContract, GetImportRequestDetailDTO>();

            // Inventory Event mappings (use SKUCode)
            CreateMap<ImportDetail, AddQuantityFromImportEvent>();
            CreateMap<ImportDetail, CreateInventoryEvent>();

            // Supplier from Excel mappings
            CreateMap<ReadImportRowFromExcelDTO, CreateSupplierFromExcelDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SupplierName));
            CreateMap<CreateSupplierFromExcelDTO, Supplier>();

            // Medicine from Excel mappings (for BulkImportFromExcelAsync)
            CreateMap<ReadImportRowFromExcelDTO, CreateMedicineFromExcelEventItem>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MedicineName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<CreateMedicineDataContractItem, GetMedicineByIdContract>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MedicineId));

            CreateMap<ReadImportRowFromExcelDTO, UpsertImportDetailDTO>();
            CreateMap<ReadImportRowFromExcelDTO, CreateImportDTO>();

            // ImportRequest mappings
            CreateMap<CreateImportRequestDTO, ImportRequest>()
                .ForMember(dest => dest.ImportRequestDetails, opt => opt.MapFrom(src => src.Details));
            CreateMap<CreateImportRequestDetailDTO, ImportRequestDetail>();

            CreateMap<ImportRequest, GetImportRequestDTO>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ImportRequestDetails))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<ImportRequestDetail, GetImportRequestDetailDTO>()
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.MedicineSnapshot != null ? src.MedicineSnapshot.MedicineName : null))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.MedicineSnapshot != null ? src.MedicineSnapshot.UnitPrice : null));

            CreateMap<UpdateImportRequestDTO, ImportRequest>()
                .ForMember(dest => dest.ImportRequestDetails, opt => opt.MapFrom(src => src.Details));
            CreateMap<UpdateImportRequestDetailDTO, ImportRequestDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImportRequestId, opt => opt.Ignore())
                .ForMember(dest => dest.ImportRequest, opt => opt.Ignore());

            // ImportRequest to Import mappings (for approval)
            CreateMap<ImportRequest, Import>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImportDetails, opt => opt.Ignore())
                .ForMember(dest => dest.ImportDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ManagerId, opt => opt.Ignore())
                .ForMember(dest => dest.ImportRequestId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ImportRequestDetail, ImportDetail>()
                .ForMember(dest => dest.ImportId, opt => opt.Ignore())
                .ForMember(dest => dest.Import, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.RequestQuantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());

            // Map from DTO to statistics contracts
            CreateMap<SupplierStatisticsDTO, GetSupplierStatisticsByDateRangeContract>();
            CreateMap<ImportStatisticsDTO, GetImportStatisticsByDateRangeContract>();
        }
    }
}