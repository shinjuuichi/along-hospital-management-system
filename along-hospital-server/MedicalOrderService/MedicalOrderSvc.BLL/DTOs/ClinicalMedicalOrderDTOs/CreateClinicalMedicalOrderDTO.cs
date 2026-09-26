using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs
{
    public class CreateClinicalMedicalOrderDTO : CreateMedicalOrderDTO
    {
        public List<CreateClinicalMedicalOrderDetailDTO> ClinicalMedicalOrderDetails { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateClinicalMedicalOrderDTO, ClinicalMedicalOrder>()
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }

    public class CreateClinicalMedicalOrderDetailDTO : MapTo<ClinicalMedicalOrderDetail>
    {
        public int Quantity { get; set; }

        public int MedicalServiceId { get; set; }
    }
}