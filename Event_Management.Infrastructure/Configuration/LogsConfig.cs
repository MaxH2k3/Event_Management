using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Configuration
{
    public static class LogsConfig
    {
        public static void AddLogs(this WebApplicationBuilder builder)
        {
            // Set up logs for everyone
            /*builder.Services.AddWatchDogServices(opt =>
            {
                opt.IsAutoClear = true;
                opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Hourly;
                opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("WatchDog");
                opt.DbDriverOption = WatchDogDbDriverEnum.Mongo;
            });*/

            // Set up logs for server
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
