using AuthSvc.BLL.Utils;
using AuthSvc.DAL.Enums;
using AuthSvc.DAL.Models;
using AutoMapper;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Enums;
using SharedLibrary.Utils;

namespace AuthSvc.BLL.DTOs
{
    public class CreateAuthDTO : MapTo<AuthAccount>
    {
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int UserId { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAuthDTO, AuthAccount>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => CryptoUtil.EncryptPassword(StringUtil.GenerateRandomPassword())))
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => AuthStageEnum.Done))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AuthStatusEnum.Verified))
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }
}
