using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using StaffSvc.BLL.Implements;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Data;

namespace StaffSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<StaffDbContext>(configuration);

            services.AddAutoMapper(typeof(StaffMappingProfile).Assembly);

            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffCertificateTypeService, StaffCertificateTypeService>();
            services.AddScoped<IStaffCertificateService, StaffCertificateService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IQualificationService, QualificationService>();
            services.AddScoped<IStaffGroupService, StaffGroupService>();
            services.AddScoped<IRegionalWageService, RegionalWageService>();
            services.AddScoped<IStaffContractService, StaffContractService>();

            return services;
        }
    }
}