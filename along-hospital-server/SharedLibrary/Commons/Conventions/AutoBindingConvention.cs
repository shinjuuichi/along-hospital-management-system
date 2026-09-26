using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SharedLibrary.Commons.Conventions;

public sealed class AutoBindingConvention : IActionModelConvention
{
    private static readonly Regex RouteParamRegex =
        new("{([^}:]+)(:[^}]*)?}", RegexOptions.Compiled);

    public void Apply(ActionModel action)
    {
        var httpMethods = action.Selectors
            .SelectMany(s => s.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                              ?? Enumerable.Empty<HttpMethodActionConstraint>())
            .SelectMany(c => c.HttpMethods)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        bool isPureGet = httpMethods.Length == 1 &&
                         httpMethods[0].Equals("GET", StringComparison.OrdinalIgnoreCase);

        var routeParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var selector in action.Selectors)
        {
            var template = selector.AttributeRouteModel?.Template ?? string.Empty;
            foreach (Match m in RouteParamRegex.Matches(template))
            {
                var name = m.Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    routeParams.Add(name);
                }
            }
        }

        foreach (var parameter in action.Parameters)
        {
            if (parameter.Attributes.Any(a => a is IBindingSourceMetadata))
            {
                continue;
            }

            var t = parameter.ParameterType;
            if (t == typeof(HttpContext) || t == typeof(ClaimsPrincipal) || t == typeof(CancellationToken))
            {
                continue;
            }

            var name = parameter.ParameterName ?? string.Empty;

            if (routeParams.Contains(name))
            {
                parameter.BindingInfo = BindingInfo.GetBindingInfo([new FromRouteAttribute()]);
                continue;
            }

            if (isPureGet)
            {
                parameter.BindingInfo = BindingInfo.GetBindingInfo([new FromQueryAttribute()]);
            }
            else
            {
                parameter.BindingInfo = BindingInfo.GetBindingInfo([new FromFormAttribute()]);
            }
        }
    }
}
