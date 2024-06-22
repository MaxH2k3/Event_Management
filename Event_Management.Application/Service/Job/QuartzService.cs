using Event_Management.Application.Dto;
using Event_Management.Domain.Enum.Events;
using Event_Management.Domain.Helper;
using Quartz;

namespace Event_Management.Application.Service.Job
{
    public interface IQuartzService
    {
        public Task StartEventStatusToOngoingJob(string eventId, DateTime startTime);
        public Task StartEventStatusToEndedJob(string eventId, DateTime startTime);
    }
    public class QuartzService : IQuartzService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public QuartzService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }
        public async Task StartEventStatusToOngoingJob(string eventId, DateTime startTime)
        {
            var jobKey = new JobKey("EventStatusToOngoingJob" + eventId);
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            IJobDetail job = JobBuilder.Create<EventStatusToOngoingJob>()
            .WithIdentity(jobKey)
            .Build();
            /*var triggers = await scheduler.GetTriggersOfJob(jobKey);
            var trigger = triggers.ElementAt(0);*/
            var newTrigger = //trigger.GetTriggerBuilder()
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime.AddMinutes(1))))
                .Build();
            //await scheduler.ScheduleJob(trigger.Key, newTrigger);
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob: Event status changed to Ongoing with id {eventId}");
            //await scheduler.TriggerJob(jobKey);
        }
        public async Task StartEventStatusToEndedJob(string eventId, DateTime startTime)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("EventStatusToEndedJob" + eventId);
            IJobDetail job = JobBuilder.Create<EventStatusToOngoingJob>()
            .WithIdentity(jobKey)
            .Build();
            /*var triggers = await scheduler.GetTriggersOfJob(jobKey);
            var trigger = triggers.ElementAt(0);*/
            var newTrigger = //trigger.GetTriggerBuilder()
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime.AddMinutes(1))))
                .Build();
            //await scheduler.RescheduleJob(trigger.Key, newTrigger);
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob: Event status changed to Ended with id {eventId}");
            //await scheduler.TriggerJob(jobKey);
        }
    }
   
}