using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class SeedDataConfigurator
    {
        public static void ConfigureSeedData(ModelBuilder modelBuilder)
        {
            var seedBuilderType = typeof(ISeedBuilder);
            var seedBuilders = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return []; }
                })
                .Where(t => seedBuilderType.IsAssignableFrom(t)
                            && t is { IsAbstract: false, IsInterface: false }
                            && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .OfType<ISeedBuilder>()
                .OrderBy(s => s.Priority)
                .ToList();

            foreach (var seed in seedBuilders)
            {
                seed.Seed(modelBuilder);
            }
        }
    }
}
