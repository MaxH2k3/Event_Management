﻿using Quartz;

namespace Event_Management.Application.Service
{
    public class EventStatusToOngoingJob : IJob
    {
        private readonly IEventService _eventService;
        private readonly ISchedulerFactory _schedulerFactory;

        public EventStatusToOngoingJob(IEventService eventService, ISchedulerFactory schedulerFactory)
        {
            _eventService = eventService;
            _schedulerFactory = schedulerFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;

            _eventService.UpdateEventStatusOngoing(Guid.Parse(currentJob.Key.ToString().Substring(14)));
            //_eventService.UpdateEventStatusEnded(currentJob.Key.ToString());
            Console.WriteLine("Task run: Event status changed to Ongoing!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ongoing Job: {currentJob.Key}");
        }
    }
}
