namespace EmailSvc.Services
{
    public interface IEmailTemplateService
    {
        Task<string> RenderTemplateAsync(string templateName, params object[] parameters);

        Task<string> RenderTemplateAsync(string templateName, IDictionary<string, string?> placeholders);
    }
}
