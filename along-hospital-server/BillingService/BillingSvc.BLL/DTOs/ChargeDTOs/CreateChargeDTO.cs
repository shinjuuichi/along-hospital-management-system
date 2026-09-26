using AutoMapper;
using BillingSvc.BLL.DTOs.RefundDTOs;
using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace BillingSvc.BLL.DTOs.ChargeDTOs
{
    public class CreateChargeDTO : MapTo<Charge>
    {
        public int MedicalServiceId { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public string? ChargeType { get; set; }

        [JsonIgnore]
        public double UnitPrice { get; set; }

        [JsonIgnore]
        public CreateChargeSnapshotDTO? ChargeSnapshot { get; set; }

        [JsonIgnore]
        public CreateRefundDTO? Refund { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateChargeDTO, Charge>()
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }
}