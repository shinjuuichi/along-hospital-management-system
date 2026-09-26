using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class SoftDeleteQueryFilter
    {
        public static void ConfigureQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, nameof(AuditEntity.IsDeleted));
                    var condition = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    // Lambda for (e => !e.IsDeleted)
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
