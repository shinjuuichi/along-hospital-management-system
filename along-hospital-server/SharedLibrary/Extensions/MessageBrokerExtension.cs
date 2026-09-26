using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using System.Reflection;

namespace SharedLibrary.Extensions
{
    public static class MessageBrokerExtension
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services,
            AppConfiguration configuration,
            Assembly? assembly = null)
        {
            services.AddScoped<IMessageBus, MessageBus>();

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration.RabbitMQConfig.Host!), h =>
                    {
                        h.Username(configuration.RabbitMQConfig.UserName);
                        h.Password(configuration.RabbitMQConfig.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        public static IServiceCollection AddMessageBroker<TDbContext>(
            this IServiceCollection services,
            AppConfiguration configuration,
            Assembly? assembly = null)
            where TDbContext : DbContext
        {
            services.AddScoped<IMessageBus, MessageBus>();

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration.RabbitMQConfig.Host!), h =>
                    {
                        h.Username(configuration.RabbitMQConfig.UserName);
                        h.Password(configuration.RabbitMQConfig.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
