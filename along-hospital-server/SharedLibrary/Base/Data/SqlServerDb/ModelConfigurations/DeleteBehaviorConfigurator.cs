using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using System.Reflection;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class DeleteBehaviorConfigurator
    {
        public static void ConfigureNavigationDeleteBehaviors(ModelBuilder modelBuilder)
        {
            // Set all foreign keys to Restrict by default
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Override with custom delete behaviors from attributes
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                var navigationProperties = clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<OnDeleteAttribute>() != null)
                    .ToList();

                foreach (var property in navigationProperties)
                {
                    var attribute = property.GetCustomAttribute<OnDeleteAttribute>()!;
                    var deleteBehavior = attribute.Mode switch
                    {
                        OnDeleteBehavior.Restrict => DeleteBehavior.Restrict,
                        OnDeleteBehavior.Cascade => DeleteBehavior.Cascade,
                        OnDeleteBehavior.SetNull => DeleteBehavior.SetNull,
                        OnDeleteBehavior.NoAction => DeleteBehavior.NoAction,
                        _ => DeleteBehavior.Restrict
                    };

                    var navigation = entityType.FindNavigation(property.Name);
                    if (navigation != null)
                    {
                        var foreignKey = navigation.ForeignKey;
                        foreignKey.DeleteBehavior = deleteBehavior;
                    }
                    else
                    {
                        var skipNavigation = entityType.FindSkipNavigation(property.Name);
                        if (skipNavigation?.ForeignKey != null)
                        {
                            skipNavigation.ForeignKey.DeleteBehavior = deleteBehavior;
                        }
                    }
                }
            }
        }
    }
}
