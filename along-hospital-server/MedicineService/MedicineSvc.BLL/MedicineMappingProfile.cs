using MedicineSvc.BLL.DTOs.MedicineCategoryDTOs;
using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MedicineSvc.BLL.DTOs.MedicineUnitDTOs;
using MedicineSvc.DAL.Models;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace MedicineSvc.BLL
{
    public class MedicineMappingProfile : BaseMappingProfile
    {
        public MedicineMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<CreateMedicineDTO, GetMedicineDTO>();
            CreateMap<UpdateMedicineAndInventoryDTO, UpdateInventoryEvent>();
            CreateMap<GetInventoryBySKUCodeContract, GetInventoryDTO>();
            CreateMap<GetMedicineSKUDTO, PreviewMedicineDetailEvent>();
            CreateMap<CreateMedicineFromExcelEventItem, CreateMedicineFromExcelDTO>();

            CreateMap<GetMedicineDTO, PreviewMedicineDetailEvent>()
                .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.Id));

            CreateMap<GetMedicineDTO, GetMedicineByIdContract>()
                .ForMember(dest => dest.MedicineImages,
                    opt => opt.MapFrom(src => src.Images ?? Array.Empty<string>()))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.MedicineCategory != null ? src.MedicineCategory.Name : null))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.MedicineCategory != null ? src.MedicineCategory.Id : 0))
                .ForMember(dest => dest.MedicineUnit,
                    opt => opt.MapFrom(src => src.MedicineUnit != null ? src.MedicineUnit.Name : null));

            CreateMap<GetMedicineByIdContract, GetMedicineDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.MedicineImages))
                .ForMember(dest => dest.MedicineUnit,
                    opt => opt.MapFrom(src =>
                     string.IsNullOrWhiteSpace(src.MedicineUnit)
                        ? null
                        : new GetMedicineUnitDTO
                        {
                            Name = src.MedicineUnit
                        }));

            CreateMap<GetMedicineDTO, GetAllMedicinesContractItem>()
                .ForMember(dest => dest.MedicineCategoryName,
                    opt => opt.MapFrom(src => src.MedicineCategory != null ? src.MedicineCategory.Name : null))
                .ForMember(dest => dest.MedicineUnit,
                    opt => opt.MapFrom(src => src.MedicineUnit != null ? src.MedicineUnit.Name : null));

            CreateMap<GetMedicineCategoryDTO, GetAllMedicineCategoriesContractItem>();

            CreateMap<Medicine, CreateMedicineDataContractItem>()
                .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.Id));

            CreateMap<MedicineSKU, GetMedicineSKUDTO>()
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Medicine != null ? src.Medicine.IsPublic : false));

            CreateMap<GetMedicineSKUDTO, GetMedicineSKUContract>()
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine != null ? src.Medicine.Name : null))
                .ForMember(dest => dest.MedicineBrand, opt => opt.MapFrom(src => src.Medicine != null ? src.Medicine.Brand : null))
                .ForMember(dest => dest.MedicineUnit, opt => opt.MapFrom(src => src.Medicine != null && src.Medicine.MedicineUnit != null ? src.Medicine.MedicineUnit.Name : null))
                .ForMember(dest => dest.MedicineImages, opt => opt.MapFrom(src => src.Medicine != null ? src.Medicine.Images : null))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Medicine != null && src.Medicine.MedicineCategory != null ? src.Medicine.MedicineCategory.Id : 0))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Medicine != null && src.Medicine.MedicineCategory != null ? src.Medicine.MedicineCategory.Name : null))
                .ForMember(dest => dest.SkuValues, opt => opt.MapFrom(src => src.SKUValues))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Medicine != null ? src.Medicine.IsPublic : false))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Inventory != null ? src.Inventory.Quantity : 0));

            CreateMap<GetSKUValueDTO, SKUValueContractItem>()
                .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.OptionValue != null && src.OptionValue.Option != null ? src.OptionValue.Option.OptionName : null))
                .ForMember(dest => dest.ValueName, opt => opt.MapFrom(src => src.OptionValue != null ? src.OptionValue.ValueName : null))
                .ForMember(dest => dest.UnitMultiplier, opt => opt.MapFrom(src => src.OptionValue != null ? src.OptionValue.UnitMultiplier : 0));
        }
    }
}
