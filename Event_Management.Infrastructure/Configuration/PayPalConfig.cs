using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayPalCheckoutSdk.Core;

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

            services.AddHttpClient("PayPal", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("Paypal:Url").Get<string>()!); // Use "https://api.paypal.com" for live environment
            });

        }
    }
}
