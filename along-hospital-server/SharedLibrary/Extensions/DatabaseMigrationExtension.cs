using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.EntityAnnotations;
using System.Reflection;

namespace SharedLibrary.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static async Task EnsureDatabaseCreatedAsync<TContext>(this WebApplication app)
            where TContext : BaseDbContext
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
                logger.LogError(ex, "[MIGRATION ERROR] Failed to migrate database for context: {ContextName}", typeof(TContext).Name);
            }
        }

        public static async Task EnsureMongoDbCreatedAsync<TContext>(this WebApplication app)
            where TContext : BaseMongoDbContext
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

                var contextAssembly = typeof(TContext).Assembly;
                var seedTypes = contextAssembly.GetTypes()
                    .Where(t => typeof(IMongoDataSeed).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .Select(t => Activator.CreateInstance(t))
                    .OfType<IMongoDataSeed>()
                    .ToList();

                if (seedTypes.Any())
                {
                    foreach (var seed in seedTypes)
                    {
                        await seed.SeedAsync(scope.ServiceProvider);
                    }

                    logger.LogInformation("[MONGO SEED] Successfully seeded data for context: {ContextName}", typeof(TContext).Name);
                }
            }
            catch (Exception ex)
            {
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
                logger.LogError(ex, "[SEED ERROR] Failed to seed data for context: {ContextName}", typeof(TContext).Name);
            }
        }
    }
}
