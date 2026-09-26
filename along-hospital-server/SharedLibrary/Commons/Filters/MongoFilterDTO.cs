using MongoDB.Driver;
using System.Reflection;

namespace SharedLibrary.Commons.Filters
{
    public class MongoFilterDTO
    {
        public int Page { get; set; } = 1;

        public virtual int PageSize { get; set; } = 10;

        public virtual string Sort { get; set; } = "CreationDate desc";

        public FilterDefinition<T>? BuildFilter<T>() where T : class
        {
            var builder = Builders<T>.Filter;
            var filters = new List<FilterDefinition<T>>();

            foreach (var prop in GetType().GetProperties())
            {
                var attribute = prop.GetCustomAttribute<FilterFieldAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                var value = prop.GetValue(this);
                if (value == null || IsDefaultValue(value))
                {
                    continue;
                }

                var fieldName = string.IsNullOrWhiteSpace(attribute.TargetField)
                    ? prop.Name
                    : attribute.TargetField;

                var filter = BuildFilterDefinition(builder, fieldName, value, attribute.Operation);
                if (filter != null)
                {
                    filters.Add(filter);
                }
            }

            return filters.Count == 0 ? null : builder.And(filters);
        }

        private FilterDefinition<T>? BuildFilterDefinition<T>(
            FilterDefinitionBuilder<T> builder,
            string fieldName,
            object value,
            FilterOperationEnum operation) where T : class
        {
            return operation switch
            {
                FilterOperationEnum.Equal => builder.Eq(fieldName, value),
                FilterOperationEnum.NotEqual => builder.Ne(fieldName, value),
                FilterOperationEnum.GreaterThan => builder.Gt(fieldName, value),
                FilterOperationEnum.LessThan => builder.Lt(fieldName, value),
                FilterOperationEnum.GreaterThanOrEqual => builder.Gte(fieldName, value),
                FilterOperationEnum.LessThanOrEqual => builder.Lte(fieldName, value),
                FilterOperationEnum.Contains when value is string strVal =>
                    builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression(strVal, "i")),
                FilterOperationEnum.NotContains when value is string strVal =>
                    builder.Not(builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression(strVal, "i"))),
                FilterOperationEnum.StartsWith when value is string strVal =>
                    builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"^{strVal}", "i")),
                FilterOperationEnum.NotStartsWith when value is string strVal =>
                    builder.Not(builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"^{strVal}", "i"))),
                FilterOperationEnum.EndsWith when value is string strVal =>
                    builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"{strVal}$", "i")),
                FilterOperationEnum.NotEndsWith when value is string strVal =>
                    builder.Not(builder.Regex(fieldName, new MongoDB.Bson.BsonRegularExpression($"{strVal}$", "i"))),
                _ => null
            };
        }

        private bool IsDefaultValue(object value)
        {
            var type = value.GetType();
            return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
        }

        public SortDefinition<T>? BuildSort<T>() where T : class
        {
            if (string.IsNullOrWhiteSpace(Sort))
            {
                return null;
            }

            var builder = Builders<T>.Sort;
            var sortParts = Sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var sortDefinitions = new List<SortDefinition<T>>();

            foreach (var part in sortParts)
            {
                var trimmedPart = part.Trim();
                var tokens = trimmedPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length == 0)
                {
                    continue;
                }

                var fieldName = tokens[0];
                var isDescending = tokens.Length > 1 &&
                    tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                sortDefinitions.Add(isDescending
                    ? builder.Descending(fieldName)
                    : builder.Ascending(fieldName));
            }

            return sortDefinitions.Count == 0 ? null : builder.Combine(sortDefinitions);
        }
    }
}
