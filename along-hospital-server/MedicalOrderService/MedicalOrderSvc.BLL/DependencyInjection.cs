using MedicalOrderSvc.BLL.Implements;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace MedicalOrderSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddMongoDatabase<MedicalOrderDbContext>(configuration);
            services.AddScoped<IMedicalOrderService, MedicalOrderService>();
            services.AddScoped<IInfusionMedicalOrderService, InfusionMedicalOrderService>();
            services.AddScoped<IClinicalMedicalOrderService, ClinicalMedicalOrderService>();
            services.AddScoped<IInstructionMedicalOrderService, InstructionMedicalOrderService>();
            services.AddAutoMapper(typeof(MedicalOrderMappingProfile).Assembly);
            return services;
        }
    }
}
