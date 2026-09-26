using Microsoft.Extensions.DependencyInjection;
using PatientSvc.BLL.Implements;
using PatientSvc.BLL.Interfaces;
using PatientSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace PatientSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<PatientDbContext>(configuration);
            services.AddAutoMapper(typeof(PatientMappingProfile).Assembly);
            services.AddScoped<IPatientService, PatientService>();
            return services;
        }
    }
}
