using CartSvc.BLL.DTOs;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace CartSvc.BLL
{
    public class CartMappingProfile : BaseMappingProfile
    {
        public CartMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<GetMedicineByIdContract, GetMedicineDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.MedicineImages));

            CreateMap<GetMedicineSKUContract, GetMedicineSKUDTO>();

            CreateMap<GetMedicineSKUContract, PreviewMedicineDetailEvent>();
        }
    }
}
