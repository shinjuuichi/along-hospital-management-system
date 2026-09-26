using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace ProductSvc.BLL
{
    public class ProductMappingProfile : BaseMappingProfile
    {
        public ProductMappingProfile()
            : base(Assembly.GetExecutingAssembly())
        {
        }
    }
}
