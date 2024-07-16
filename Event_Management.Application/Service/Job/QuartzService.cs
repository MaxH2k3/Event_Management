﻿using Event_Management.Application.Dto;
using Event_Management.Domain.Enum.Events;
using Event_Management.Domain.Helper;
using Quartz;
using Quartz.Impl.Matchers;

namespace Event_Management.Application.Service.Job
{
    public interface IQuartzService
    {
        Task StartEventStatusToOngoingJob(Guid eventId, DateTime startTime);
        Task StartEventStatusToEndedJob(Guid eventId, DateTime startTime);
        Task StartEventStartingEmailNoticeJob(Guid eventId, DateTime startTime);
        Task StartEventEndingEmailNoticeJob(Guid eventId, DateTime endTime);
        Task DeleteJobsByEventId(string eventId);
        Task<IEnumerable<string>> GetAllJob();
        Task ExcuteJob(string key, int type);

    }
    public class QuartzService : IQuartzService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public QuartzService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<IEnumerable<string>> GetAllJob()
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var allJobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            return allJobKeys.Select(jobKey => jobKey.Name);
        }

        public async Task ExcuteJob(string key, int type)
        {
            var eventId = Guid.Parse(key);
            switch (type)
            {
                case 1:
                {
                    await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                    break;
                }
                case 2:
                {
                    await StartEventStatusToEndedJob(eventId, DateTime.Now.AddMinutes(1));
                    break;
                }
                case 3:
                {
                    await StartEventStartingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                    break;
                }
                case 4:
                {
                    await StartEventEndingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                    break;
                }
                case 5:
                {
                    await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                    await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                    await StartEventStartingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                    await StartEventEndingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                    break;
                } default:
                {
                    break;
                }
            }
                
        }

        public async Task StartEventStatusToOngoingJob(Guid eventId, DateTime startTime)
        {
            var jobKey = new JobKey("start-" + eventId);
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
            Console.WriteLine($"ScheduleJob: Event status changed to Ongoing with id {jobKey}");
            //await scheduler.TriggerJob(jobKey);
        }
        public async Task StartEventStatusToEndedJob(Guid eventId, DateTime startTime)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("ended-" + eventId);
            IJobDetail job = JobBuilder.Create<EventStatusToEndedJob>()
            .WithIdentity(jobKey)
            .Build();
            var newTrigger =
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime.AddMinutes(1))))
                .Build();
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob: Event status changed to Ended with id {jobKey}");
        }

        public async Task StartEventStartingEmailNoticeJob(Guid eventId, DateTime startTime)
        {
            var jobKey = new JobKey("E-start-" + eventId);
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            IJobDetail job = JobBuilder.Create<EventStartingEmailJob>()
            .WithIdentity(jobKey)
            .Build();
            var newTrigger =
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime)))
                .Build();
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob:  Event starting notice email with id {jobKey}");
        }
        public async Task StartEventEndingEmailNoticeJob(Guid eventId, DateTime endTime)
        {
            var jobKey = new JobKey("E-ended-" + eventId);
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            IJobDetail job = JobBuilder.Create<EventEndingEmailJob>()
            .WithIdentity(jobKey)
            .Build();
            var newTrigger =
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(endTime)))
                .Build();
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob:  Event ending notice email with id {jobKey}");
        }
        public async Task DeleteJobsByEventId(string eventId)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var allJobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            foreach (var jobKey in allJobKeys)
            {
                if (jobKey.Name.Contains(eventId))
                {
                    Console.WriteLine("Delete JobKey: " +  jobKey.Name);
                    await scheduler.DeleteJob(jobKey);
                    
                }
            }
        }
    }

}