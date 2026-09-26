using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.ChargeDTOs
{
    public class CreateChargeSnapshotDTO : MapTo<ChargeSnapshot>
    {
        public string? MedicalServiceName { get; set; }

        public string? MedicalServiceCode { get; set; }

        public string? MedicalServiceDescription { get; set; }
    }
}