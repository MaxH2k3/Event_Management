using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class RedisConfig
    {
        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = configuration["Redis:HostName"];
                var redisPassword = configuration["Redis:Password"];
                options.Configuration = $"{redisConnection},password={redisPassword}";
            });
            services.AddDistributedMemoryCache();
        }
    }
}
