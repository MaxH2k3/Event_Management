﻿using Quartz;

namespace Event_Management.Application.Service
{
    public class AllEventStatusToEndedJob : IJob
    {
        private readonly IEventService _eventService;
        private readonly ISchedulerFactory _schedulerFactory;
        public AllEventStatusToEndedJob(IEventService eventService, ISchedulerFactory schedulerFactory)
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
            _eventService.UpdateEventStatusEnded();
            Console.WriteLine("Task run: Event status changed to Ended!");
            
            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ended Job: {currentJob.Key}");
            //return Task.CompletedTask;
        }
    }
}
