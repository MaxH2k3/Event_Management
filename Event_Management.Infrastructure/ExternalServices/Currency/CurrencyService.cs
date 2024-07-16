using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.ExternalServices.Currency
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> ConvertCurrency(string value)
        {
            var requestUrl = $"https://api.currencyapi.com/v3/convert?value={value}&from=USD&to=VND";
            _httpClient.DefaultRequestHeaders.Add("apikey", _configuration["CurrencyApi:ApiKey"]);

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
