using BlogSvc.BLL.Implements;
using BlogSvc.BLL.Interfaces;
using BlogSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace BlogSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<BlogDbContext>(configuration);
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddAutoMapper(typeof(BlogMappingProfile).Assembly);
            return services;
        }
    }
}