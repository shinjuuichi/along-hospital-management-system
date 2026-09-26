using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using MedicalOrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs
{
    public class GetInfusionMedicalOrderDTO : GetMedicalOrderDTO, IMapFrom<InfusionMedicalOrder>
    {
        public List<GetInfusionMedicalOrderDetailDTO> InfusionMedicalOrderDetails { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InfusionMedicalOrder, GetInfusionMedicalOrderDTO>();
        }
    }

    public class GetInfusionMedicalOrderDetailDTO : MapFrom<InfusionMedicalOrderDetail>
    {
        public string? Id { get; set; }

        public string? Rate { get; set; }

        public string? Frequency { get; set; }

        public string? Duration { get; set; }

        public string? InfusionMedicalOrderDetailExecutionStatus { get; set; }

        public string? Note { get; set; }

        public int MedicineId { get; set; }

        public GetMedicineSnapshotDTO? MedicineSnapshot { get; set; }
    }

    public class GetMedicineSnapshotDTO : MapFrom<MedicineSnapshot>
    {
        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? MedicineUnit { get; set; }

        public string? MedicineImage { get; set; }

        public string? CategoryName { get; set; }
    }
}