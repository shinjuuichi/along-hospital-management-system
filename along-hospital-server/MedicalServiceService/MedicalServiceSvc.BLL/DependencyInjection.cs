using MedicalServiceSvc.BLL.Implements;
using MedicalServiceSvc.BLL.Interfaces;
using MedicalServiceSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace MedicalServiceSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<MedicalServiceDbContext>(configuration);
            services.AddAutoMapper(typeof(MedicalServiceMappingProfile).Assembly);
            services.AddScoped<IMedicalServiceService, MedicalServiceService>();
            return services;
        }
    }
}
