using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using MedicalOrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs
{
    public class GetClinicalMedicalOrderDTO : GetMedicalOrderDTO, IMapFrom<ClinicalMedicalOrder>
    {
        public string? ClinicalMedicalOrderStatus { get; set; }

        public int PendingInvoiceId { get; set; }

        public List<GetClinicalMedicalOrderDetailDTO> ClinicalMedicalOrderDetails { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ClinicalMedicalOrder, GetClinicalMedicalOrderDTO>();
        }
    }

    public class GetClinicalMedicalOrderDetailDTO : MapFrom<ClinicalMedicalOrderDetail>
    {
        public string? Id { get; set; }

        public int Quantity { get; set; }

        public string? ClinicalMedicalOrderDetailStatus { get; set; }

        public int MedicalServiceId { get; set; }

        public GetMedicalServiceSnapshotDTO? MedicalServiceSnapshot { get; set; }
    }

    public class GetMedicalServiceSnapshotDTO : MapFrom<MedicalServiceSnapshot>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Code { get; set; }
    }
}