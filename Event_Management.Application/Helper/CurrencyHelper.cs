using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Helper
{
    public class CurrencyHelper
    {
        public static async Task<string> GetExchangeRate(string url, decimal? amount)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseBody);


                decimal exchangeRate = data["data"]["VND"]["value"].Value<decimal>();

                // Tính toán số tiền sau khi chuyển đổi
                decimal translatedAmount = (decimal)amount / exchangeRate;
                return translatedAmount.ToString("F2");
            }
        }
    }
}
