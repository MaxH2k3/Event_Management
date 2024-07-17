using Event_Management.Domain.Models.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class CurrencyConfig
    {
        public static void UpdateCurrency(this IServiceCollection services, IConfiguration configuration)
        {

            configuration.GetSection("CurrencyApi:ApiKey").Get<string>();

        }
    }
}
