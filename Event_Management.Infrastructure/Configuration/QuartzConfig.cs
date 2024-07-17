using Event_Management.Application.Service;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Event_Management.Infrastructure.Configuration
{
    public static class QuartzConfig
    {
        public static void AddQuartz(this IServiceCollection services)
        {
            services.AddSingleton<StdSchedulerFactory>();

            services.AddQuartz(q =>
            {
                //q.UseMicrosoftDependencyInjectionJobFactory();
                //name of your job that you created in the Jobs folder.
                var jobKey = new JobKey("AllEventStatusToEndedJob");
                var jobKey2 = new JobKey("AllEventStatusToOngoingJob");
                q.AddJob<AllEventStatusToOngoingJob>(opts => opts.WithIdentity(jobKey2));
                q.AddJob<AllEventStatusToEndedJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts.ForJob(jobKey2)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );
                q.AddTrigger(opts => opts.ForJob(jobKey)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );

            });

            services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
                q.AwaitApplicationStarted = true;
            });
        }
    }
}
