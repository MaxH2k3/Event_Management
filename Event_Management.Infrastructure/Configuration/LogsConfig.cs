using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using WatchDog;
using WatchDog.src.Enums;

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
