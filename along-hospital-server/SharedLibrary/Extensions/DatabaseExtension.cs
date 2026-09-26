using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons;
using SharedLibrary.Services.Implements;
using SharedLibrary.Services.Implements.CacheServices;
using SharedLibrary.Services.Interfaces;
using StackExchange.Redis;

namespace SharedLibrary.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddApplicationDbContext<TContext>(
          this IServiceCollection services,
          AppConfiguration configuration)
          where TContext : BaseDbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(configuration.DatabaseConfig.ConnectionString));

            services.AddScoped<IUnitOfWork>(provider =>
                new UnitOfWork(
                    provider.GetRequiredService<TContext>(),
                    provider.GetRequiredService<ICurrentUserService>()
                ));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        public static IServiceCollection AddRedisDatabase(this IServiceCollection services, AppConfiguration configuration)
        {
            var redisConnectionString = string.IsNullOrWhiteSpace(configuration.RedisConfig?.Password)
            ? configuration.RedisConfig?.Host
            : $"{configuration.RedisConfig?.Host},password={configuration.RedisConfig?.Password}";

            if (string.IsNullOrWhiteSpace(redisConnectionString))
            {
                services.AddDistributedMemoryCache();
                services.AddScoped<ICacheService, MemoryCacheService>();
                services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

                return services;
            }

            try
            {
                var options = ConfigurationOptions.Parse(redisConnectionString);
                options.AbortOnConnectFail = true;
                options.ConnectRetry = 1;
                options.ConnectTimeout = 3000;
                options.SyncTimeout = 3000;
                options.ReconnectRetryPolicy = new ExponentialRetry(3000);

                var multiplexer = ConnectionMultiplexer.Connect(options);

                services.AddSingleton<IConnectionMultiplexer>(multiplexer);

                services.AddStackExchangeRedisCache(opts =>
                {
                    opts.Configuration = redisConnectionString;
                    opts.InstanceName = configuration.RedisConfig?.InstanceName ?? string.Empty;
                });

                services.AddScoped<ICacheService, RedisCacheService>();
                services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

                return services;
            }
            catch
            {
                services.AddDistributedMemoryCache();
                services.AddScoped<ICacheService, MemoryCacheService>();
                services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

                return services;
            }
        }

        public static IServiceCollection AddMongoDatabase<TContext>(
                    this IServiceCollection services,
                    AppConfiguration configuration)
                    where TContext : BaseMongoDbContext
        {
            var connectionString = configuration.DatabaseMongoConfig.ConnectionString;
            var databaseName = configuration.DatabaseMongoConfig.DatabaseName;

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            services.AddScoped<BaseMongoDbContext, TContext>();

            services.AddScoped(typeof(IMongoGenericRepository<>), typeof(MongoGenericRepository<>));

            return services;
        }
    }
}