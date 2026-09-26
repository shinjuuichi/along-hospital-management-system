using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace BlogSvc.BLL
{
    public class BlogMappingProfile : BaseMappingProfile
    {
        public BlogMappingProfile() : base(Assembly.GetExecutingAssembly()) { }
    }
}