using BillingSvc.BLL.Implements;
using BillingSvc.BLL.Interfaces;
using BillingSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace BillingSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<BillingDbContext>(configuration);
            services.AddAutoMapper(typeof(BillingMappingProfile).Assembly);
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IBillingStatisticsService, BillingStatisticsService>();
            return services;
        }
    }
}
