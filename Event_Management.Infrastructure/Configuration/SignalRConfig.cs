using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class SignalRConfig
    {
        public static void AddRealTime(this IServiceCollection services)
        {
            services
                .AddSignalR(option =>
                {
                    option.EnableDetailedErrors = true;
                    option.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                    option.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5MB
                })
                .AddJsonProtocol(option =>
                {
                    option.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        }
    }
}
