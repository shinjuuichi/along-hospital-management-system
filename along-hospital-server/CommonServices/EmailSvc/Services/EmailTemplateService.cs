using SharedLibrary.Commons;

namespace EmailSvc.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly string _templatesFolderPath;
        private readonly string _baseTemplateFolderPath;
        private readonly AppConfiguration _configuration;
        private const string FooterToken = "{{footer}}";

        public EmailTemplateService(AppConfiguration appConfig)
        {
            _configuration = appConfig;
            _templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            _baseTemplateFolderPath = Path.Combine(_templatesFolderPath, "Base");
        }

        public async Task<string> RenderTemplateAsync(string templateName, params object[] parameters)
        {
            var templatePath = Path.Combine(_templatesFolderPath, $"{templateName}.html");
            var footerPath = Path.Combine(_baseTemplateFolderPath, "footer.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template not found: {templatePath}");
            }

            var emailTemplate = await File.ReadAllTextAsync(templatePath);
            var footerTemplate = await File.ReadAllTextAsync(footerPath);

            footerTemplate = footerTemplate.Replace("{{webUrl}}", _configuration.UrlsConfig?.FrontendUrl ?? string.Empty);

            var htmlBody = emailTemplate;
            for (int i = 0; i < parameters.Length; i++)
            {
                htmlBody = htmlBody.Replace($"{{{i}}}", parameters[i]?.ToString());
            }

            htmlBody = InjectFooter(htmlBody, footerTemplate);

            return htmlBody;
        }

        public async Task<string> RenderTemplateAsync(string templateName, IDictionary<string, string?> placeholders)
        {
            var templatePath = Path.Combine(_templatesFolderPath, $"{templateName}.html");
            var footerPath = Path.Combine(_baseTemplateFolderPath, "footer.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template not found: {templatePath}");
            }

            var emailTemplate = await File.ReadAllTextAsync(templatePath);
            var footerTemplate = await File.ReadAllTextAsync(footerPath);

            footerTemplate = footerTemplate.Replace("{{webUrl}}", _configuration.UrlsConfig?.FrontendUrl ?? string.Empty);

            var htmlBody = emailTemplate;
            if (placeholders != null)
            {
                foreach (var kv in placeholders)
                {
                    var key = kv.Key;
                    var value = kv.Value ?? string.Empty;
                    htmlBody = htmlBody.Replace($"{{{{{key}}}}}", value);
                    footerTemplate = footerTemplate.Replace($"{{{{{key}}}}}", value);
                }
            }

            htmlBody = InjectFooter(htmlBody, footerTemplate);

            return htmlBody;
        }

        private static string InjectFooter(string htmlBody, string footerHtml)
        {
            if (string.IsNullOrEmpty(footerHtml))
            {
                return htmlBody;
            }

            if (htmlBody.Contains(FooterToken, StringComparison.OrdinalIgnoreCase))
            {
                return htmlBody.Replace(FooterToken, footerHtml, StringComparison.OrdinalIgnoreCase);
            }

            const string closingBody = "</body>";
            var idx = htmlBody.LastIndexOf(closingBody, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                return htmlBody.Insert(idx, footerHtml);
            }

            return htmlBody + footerHtml;
        }
    }
}
