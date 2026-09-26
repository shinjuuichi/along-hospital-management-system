using AuthSvc.BLL.Implements;
using AuthSvc.BLL.Interfaces;
using AuthSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace AuthSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<AuthDbContext>(configuration);
            services.AddRedisDatabase(configuration);

            services.AddBaseServices();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVerificationCacheService, VerificationCacheService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IRefreshTokenCookieService, RefreshTokenCookieService>();

            services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);

            return services;
        }
    }
}
