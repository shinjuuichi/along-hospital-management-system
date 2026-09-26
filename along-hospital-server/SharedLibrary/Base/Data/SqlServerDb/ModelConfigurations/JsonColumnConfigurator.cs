using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedLibrary.Commons.EntityAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class JsonColumnConfigurator
    {
        public static void ConfigureJsonColumns(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                var jsonProps = clrType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => Attribute.IsDefined(p, typeof(JsonColumnAttribute)));

                foreach (var prop in jsonProps)
                {
                    var converter = (ValueConverter)Activator.CreateInstance(typeof(JsonValueConverter<>).MakeGenericType(prop.PropertyType))!;
                    var comparer = (ValueComparer)Activator.CreateInstance(typeof(JsonValueComparer<>).MakeGenericType(prop.PropertyType))!;

                    modelBuilder.Entity(clrType)
                        .Property(prop.Name)
                        .HasConversion(converter)
                        .Metadata
                        .SetValueComparer(comparer);
                }
            }
        }

        private sealed class JsonValueConverter<T> : ValueConverter<T?, string?>
        {
            private static readonly JsonSerializerOptions Options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };

            public JsonValueConverter() : base(
                v => v == null ? null : JsonSerializer.Serialize(v, Options),
                v => v == null ? default : JsonSerializer.Deserialize<T>(v, Options))
            { }
        }

        private sealed class JsonValueComparer<T> : ValueComparer<T?>
        {
            private static readonly JsonSerializerOptions Options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };

            public JsonValueComparer() : base(
                (l, r) => JsonEquals(l, r),
                v => v == null ? 0 : JsonHash(v),
                v => DeepClone(v))
            { }

            private static bool JsonEquals(T? left, T? right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (left is null || right is null)
                {
                    return false;
                }

                using var lDoc = JsonDocument.Parse(JsonSerializer.Serialize(left, Options));
                using var rDoc = JsonDocument.Parse(JsonSerializer.Serialize(right, Options));

                return lDoc.RootElement.Equals(rDoc.RootElement);
            }

            private static int JsonHash(T value)
            {
                var json = JsonSerializer.Serialize(value, Options);
                return json.GetHashCode();
            }

            private static T? DeepClone(T? value)
            {
                if (value is null)
                {
                    return default;
                }

                var json = JsonSerializer.Serialize(value, Options);
                return JsonSerializer.Deserialize<T>(json, Options);
            }
        }
    }
}