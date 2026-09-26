using Microsoft.Extensions.DependencyInjection;
using Net.payOS;
using SharedLibrary.Commons;
using VNPAY.NET;

namespace PaymentSvc.BLL.Extensions
{
    public static class PayOSRegistration
    {
        public static IServiceCollection AddPayOS(this IServiceCollection services, PayOSConfig config)
        {
            services.AddSingleton(new PayOS(config.ClientId, config.ApiKey, config.ChecksumKey));

            return services;
        }
    }

    public static class VnPayRegistration
    {
        public static IServiceCollection AddVnPay(this IServiceCollection services, VnPayConfig config)
        {
            services.AddSingleton(provider =>
            {
                var vn = new Vnpay(); vn.Initialize(config.TmnCode, config.HashSecret, config.BaseUrl, config.ReturnUrl);
                return vn;
            });

            return services;
        }
    }
}