using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs
{
    public class CreateInfusionMedicalOrderDTO : CreateMedicalOrderDTO
    {
        public List<CreateInfusionMedicalOrderDetailDTO> InfusionMedicalOrderDetails { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateInfusionMedicalOrderDTO, InfusionMedicalOrder>()
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }

    public class CreateInfusionMedicalOrderDetailDTO : MapTo<InfusionMedicalOrderDetail>
    {
        public string? Rate { get; set; }

        public string? Frequency { get; set; }

        public string? Duration { get; set; }

        public string? Note { get; set; }

        public int MedicineId { get; set; }
    }
}