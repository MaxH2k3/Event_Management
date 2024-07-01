using Event_Management.Domain.Models.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class PayPalConfig
    {

        public static void AddPayPal(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSingleton(sp =>
            {
                var clientId = configuration.GetSection("PayPal:ClientId").Get<string>();
                var secret = configuration.GetSection("PayPal:Secret").Get<string>();
                var environment = new SandboxEnvironment(clientId, secret);
                return new PayPalHttpClient(environment);
            });



        }
    }
}
