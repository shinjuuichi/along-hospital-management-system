using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class ForeignKeyValidator
    {
        public static async Task ValidateForeignKeysAsync(
            DbContext context,
            CancellationToken cancellationToken = default)
        {
            var addedOrModifiedEntries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in addedOrModifiedEntries)
            {
                var entityType = context.Model.FindEntityType(entry.Entity.GetType());
                if (entityType == null)
                {
                    continue;
                }

                var foreignKeys = entityType.GetForeignKeys();

                foreach (var foreignKey in foreignKeys)
                {
                    var principalType = foreignKey.PrincipalEntityType.ClrType;

                    if (!typeof(AuditEntity).IsAssignableFrom(principalType))
                    {
                        continue;
                    }

                    var fkProperties = foreignKey.Properties;
                    var allFkValuesNull = true;
                    var foreignKeyValues = new List<object?>();

                    foreach (var property in fkProperties)
                    {
                        var value = entry.Property(property.Name).CurrentValue;
                        foreignKeyValues.Add(value);
                        if (value != null)
                        {
                            allFkValuesNull = false;
                        }
                    }

                    if (allFkValuesNull)
                    {
                        continue;
                    }

                    var principalNavigation = foreignKey.DependentToPrincipal;
                    if (principalNavigation != null)
                    {
                        var principalEntity = entry.Navigation(principalNavigation.Name).CurrentValue;
                        if (principalEntity != null)
                        {
                            var principalEntry = context.Entry(principalEntity);
                            if (principalEntry.State == EntityState.Added)
                            {
                                continue;
                            }
                        }
                    }

                    var principalDbSet = context.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                            p.PropertyType.GetGenericArguments()[0] == principalType)
                        ?.GetValue(context);

                    if (principalDbSet == null)
                    {
                        continue;
                    }

                    var principalKey = foreignKey.PrincipalKey;
                    var keyValues = foreignKeyValues.ToArray();

                    var queryMethod = typeof(ForeignKeyValidator)
                        .GetMethod(nameof(CheckEntityExistsAsync), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(principalType);

                    var exists = await (Task<bool>)queryMethod.Invoke(null, new[] { context, principalDbSet, principalKey.Properties.Select(p => p.Name).ToArray(), keyValues, cancellationToken })!;

                    if (!exists)
                    {
                        var navigationName = foreignKey.DependentToPrincipal?.Name ?? principalType.Name;

                        var innerException = new InvalidOperationException(
                            $"The FOREIGN KEY constraint failed. The {navigationName} reference does not exist or has been soft-deleted.");

                        throw new DbUpdateException(
                            $"Foreign key constraint violation: The referenced {navigationName} does not exist or has been deleted.",
                            innerException);
                    }
                }
            }
        }

        private static async Task<bool> CheckEntityExistsAsync<TEntity>(
            DbContext context,
            object dbSet,
            string[] keyPropertyNames,
            object?[] keyValues,
            CancellationToken cancellationToken) where TEntity : class
        {
            var query = (IQueryable<TEntity>)dbSet;

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression? predicate = null;

            for (int i = 0; i < keyPropertyNames.Length; i++)
            {
                var property = Expression.Property(parameter, keyPropertyNames[i]);
                var constant = Expression.Constant(keyValues[i], property.Type);
                var equals = Expression.Equal(property, constant);

                predicate = predicate == null
                    ? equals
                    : Expression.AndAlso(predicate, equals);
            }

            if (predicate == null)
            {
                return false;
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);

            return await query.AnyAsync(lambda, cancellationToken);
        }
    }
}
