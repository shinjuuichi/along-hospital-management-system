using Microsoft.Extensions.DependencyInjection;
using PaymentSvc.BLL.Extensions;
using PaymentSvc.BLL.Implements;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace PaymentSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddMongoDatabase<PaymentDbContext>(configuration);

            services.AddHttpContextAccessor();

            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<ICashService, CashService>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<ISePayService, SePayService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddPayOS(configuration.PayOSConfig);
            services.AddVnPay(configuration.VnPayConfig);

            services.AddAutoMapper(typeof(PaymentMappingProfile).Assembly);

            return services;
        }
    }
}