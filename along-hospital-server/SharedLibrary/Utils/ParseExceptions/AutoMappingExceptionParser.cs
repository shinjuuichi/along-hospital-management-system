using AutoMapper;

namespace SharedLibrary.Utils.ParseExceptions
{
    public static class AutoMappingExceptionParser
    {
        public static (string? SourceType, string? DestinationType, string? DestinationMember, string Error, string? InnerError)
            Parse(AutoMapperMappingException ex)
        {
            var message = ex.Message ?? string.Empty;
            string? source = null;
            string? dest = null;
            string? destMember = null;

            try
            {
                var lines = message
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .ToArray();

                int mappingHeaderIdx = Array.FindIndex(lines, l => l.Equals("Mapping types:", StringComparison.OrdinalIgnoreCase));
                if (mappingHeaderIdx >= 0)
                {
                    for (int i = mappingHeaderIdx + 1; i < Math.Min(lines.Length, mappingHeaderIdx + 3); i++)
                    {
                        var line = lines[i];
                        var arrowIdx = line.IndexOf("->", StringComparison.Ordinal);
                        if (arrowIdx > 0)
                        {
                            var left = line[..arrowIdx].Trim();
                            var right = line[(arrowIdx + 2)..].Trim();
                            if (!string.IsNullOrEmpty(left)) source = left;
                            if (!string.IsNullOrEmpty(right)) dest = right;
                        }
                    }
                }

                int destMemberHeaderIdx = Array.FindIndex(lines, l => l.Equals("Destination Member:", StringComparison.OrdinalIgnoreCase));
                if (destMemberHeaderIdx >= 0 && destMemberHeaderIdx + 1 < lines.Length)
                {
                    destMember = lines[destMemberHeaderIdx + 1];
                }
            }
            catch (Exception parseEx)
            {
                throw new Exception(parseEx.Message);
            }

            var inner = ex.InnerException?.Message;

            var summaryParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(dest))
                summaryParts.Add($"{source} -> {dest}");
            if (!string.IsNullOrWhiteSpace(destMember))
                summaryParts.Add($"Member: {destMember}");
            var summary = summaryParts.Count > 0 ? $"Error mapping {string.Join(", ", summaryParts)}." : "Error mapping types.";

            var error = !string.IsNullOrWhiteSpace(inner)
                ? $"{summary} {inner}"
                : (string.IsNullOrWhiteSpace(message) ? summary : message);

            return (source, dest, destMember, error, inner);
        }
    }
}
