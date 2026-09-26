using System.Text.RegularExpressions;

namespace SharedLibrary.Utils.ParseExceptions
{
    public static class SqlExceptionParser
    {
        private static readonly Regex UniqueConstraintViolationRegex =
            new Regex(@"Cannot insert duplicate key row in object 'dbo\.(.+?)' with unique index '(.+?)'. The duplicate key value is \((.+?)\)\.", RegexOptions.Compiled);

        private static readonly Regex ForeignKeyConstraintViolationRegex =
            new Regex(@"The (?<operation>INSERT|UPDATE|DELETE) statement conflicted with the (?:FOREIGN KEY|REFERENCE) constraint ""(?<constraint>.+?)"". The conflict occurred in database ""(?<database>.+?)"", table ""dbo\.(?<table>.+?)"", column '(?<column>.+?)'", RegexOptions.Compiled);

        private static readonly Regex EntityTrackingRegex =
            new Regex(@"The instance of entity type '(.+?)' cannot be tracked because another instance with the same key value for \{(.+?)\} is already being tracked", RegexOptions.Compiled);

        public static (string? Field, string? Value) ParseUniqueConstraintViolation(string message)
        {
            var match = UniqueConstraintViolationRegex.Match(message);
            if (!match.Success) return (null, null);

            var indexName = match.Groups[2].Value;
            var value = match.Groups[3].Value;

            var fieldName = indexName.Split('_').LastOrDefault();
            return (fieldName, value);
        }

        public static (string? Field, string? RelatedTable, string? Operation) ParseForeignKeyConstraintViolation(string message)
        {
            var match = ForeignKeyConstraintViolationRegex.Match(message);
            if (!match.Success) return (null, null, null);

            var operation = match.Groups["operation"].Value;
            var constraintName = match.Groups["constraint"].Value;
            var relatedTable = match.Groups["table"].Value;

            var parts = constraintName.Split('_');
            var fieldName = parts.Length > 2 ? parts.LastOrDefault() : null;

            return (fieldName, relatedTable, operation);
        }

        public static (string? EntityType, List<string>? Keys) ParseEntityTrackingException(string message)
        {
            var match = EntityTrackingRegex.Match(message);
            if (!match.Success) return (null, null);

            var entityType = match.Groups[1].Value;
            var keysString = match.Groups[2].Value;

            var keys = keysString
                .Split(',')
                .Select(k => k.Trim().Trim('\'', '"'))
                .ToList();

            return (entityType, keys);
        }

        public static (string? Field, string? RelatedEntity) ParseSoftDeleteForeignKeyViolation(string message)
        {
            var match = Regex.Match(message, @"The referenced (.+?) does not exist or has been deleted");
            if (!match.Success) return (null, null);

            var entityName = match.Groups[1].Value;
            var fieldName = $"{entityName}Id";

            return (fieldName, entityName);
        }
    }
}
