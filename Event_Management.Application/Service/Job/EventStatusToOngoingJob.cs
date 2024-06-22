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
            _eventService.UpdateEventStatusOngoing();
            _eventService.UpdateEventStatusEnded();
            Console.WriteLine("Task run: Event status changed to Ongoing!");

            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob and UnscheduledJob: {currentJob.Key}");
        }
    }
}