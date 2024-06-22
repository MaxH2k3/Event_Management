using Event_Management.Domain.Models.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class StripeConfig
    {
        public static void AddStripe(this IServiceCollection services, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Get<string>();
        }
    }
}
