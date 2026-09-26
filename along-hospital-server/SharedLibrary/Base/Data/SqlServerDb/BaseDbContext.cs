using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations;

namespace SharedLibrary.Base.Data.SqlServerDb
{
    public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DeleteBehaviorConfigurator.ConfigureNavigationDeleteBehaviors(modelBuilder);
            SoftDeleteQueryFilter.ConfigureQueryFilter(modelBuilder);
            UniqueIndexConfigurator.ConfigureUniqueIndexes(modelBuilder);
            JsonColumnConfigurator.ConfigureJsonColumns(modelBuilder);

            SeedDataConfigurator.ConfigureSeedData(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            DecimalConvention.ConfigureDecimalConvention(configurationBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await SoftDeleteCascadeHandler.ApplySoftDeleteCascadeAsync(this, cancellationToken);
            await ForeignKeyValidator.ValidateForeignKeysAsync(this, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}