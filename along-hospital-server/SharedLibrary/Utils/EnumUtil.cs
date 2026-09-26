using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedLibrary.Utils
{
    public static class EnumUtil
    {
        public static string GetEnumDisplayName(this Enum @enum)
        {
            return @enum.GetType()
                        .GetMember(@enum.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? @enum.ToString();
        }

        public static T GetEnumFromString<T>(string value) where T : struct, Enum
        {
            if (Enum.TryParse(value, out T @enum))
            {
                return @enum;
            }

            throw new InvalidDataException($"Invalid enum value of type {typeof(T)}");
        }

        public static List<string> GetEnumValuesAsListString<T>()
        {
            return [.. Enum.GetValues(typeof(T)).Cast<T>().Select(e => e?.ToString() ?? string.Empty)];
        }

        public static TEnum ParseEnum<TEnum>(string? value) where TEnum : struct, Enum
        {
            if (Enum.TryParse(value, ignoreCase: true, out TEnum parsed))
            {
                return parsed;
            }

            throw new InvalidDataException($"The value '{value}' is invalid for enum type {typeof(TEnum).Name}.");
        }

        public static bool TryParse<TEnum>(string? value, out TEnum parsed) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                parsed = default;
                return false;
            }

            return Enum.TryParse(value.Trim(), ignoreCase: true, out parsed);
        }
    }
}
