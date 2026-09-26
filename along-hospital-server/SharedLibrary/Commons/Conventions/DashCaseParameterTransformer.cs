using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace SharedLibrary.Commons.Conventions
{
    public sealed class DashCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value is null) return null;
            var s = value.ToString()!;

            s = Regex.Replace(s, @"([a-z0-9])([A-Z])", "$1-$2");
            s = Regex.Replace(s, @"([A-Z]+)([A-Z][a-z])", "$1-$2");
            s = s.Replace('_', '-');

            return s;
        }
    }
}