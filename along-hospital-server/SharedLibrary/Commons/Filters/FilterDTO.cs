using SharedLibrary.Utils;
using System.Reflection;

namespace SharedLibrary.Commons.Filters
{
    public class FilterDTO
    {
        public int Page { get; set; } = 1;

        public virtual int PageSize { get; set; } = 10;

        public virtual string Sort { get; set; } = "Id desc";

        public string? Filter => BuildFilter();

        protected virtual string? BuildFilter()
        {
            List<string> filters = [];

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

                var field = string.IsNullOrWhiteSpace(attribute.TargetField) ? prop.Name : attribute.TargetField;

                string formattedValue;
                switch (value)
                {
                    case string strVal:
                        if (string.IsNullOrWhiteSpace(strVal))
                        {
                            continue;
                        }

                        strVal = strVal.Trim();
                        prop.SetValue(this, strVal);
                        formattedValue = attribute.Operation is FilterOperationEnum.Contains
                                    or FilterOperationEnum.NotContains
                                    ? $"{strVal}/i"
                                    : strVal;
                        break;
                    case DateTime dateVal:
                        formattedValue = dateVal.ToString("o");
                        break;
                    default:
                        formattedValue = value.ToString()!;
                        break;
                }

                filters.Add($"{field} {attribute.Operation.GetEnumDisplayName()} {formattedValue}");
            }

            return filters.Count != 0 ? string.Join(",", filters) : null;
        }

        private bool IsDefaultValue(object value)
        {
            var type = value.GetType();
            return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
        }
    }
}
