using PaymentSvc.BLL.DTOs.ExchangeRateDTOs;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Commons;
using System.Net.Http.Json;

namespace PaymentSvc.BLL.Implements
{
    public class ExchangeRateService(HttpClient httpClient,
        AppConfiguration appConfiguration) : IExchangeRateService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AppConfiguration _appConfiguration = appConfiguration;
        private const string ExchangeRateApiUrlTemplate = "https://v6.exchangerate-api.com/v6/{0}/pair/{1}/{2}";

        public async Task<decimal> GetExchageRate(string from, string to)
        {
            var apiKey = _appConfiguration.ExchangeRateConfig.ApiKey;
            var url = string.Format(ExchangeRateApiUrlTemplate, apiKey, from, to);

            var response = await _httpClient.GetFromJsonAsync<ExchangeRateRespondDTO>(url);

            if (response == null || response.Result != "success")
            {
                throw new InvalidOperationException("Failed to fetch exchange rate");
            }

            return response.ConversionRate;
        }
    }
}