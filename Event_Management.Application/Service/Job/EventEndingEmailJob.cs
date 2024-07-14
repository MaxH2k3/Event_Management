﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Job
{
    public class EventEndingEmailJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public EventEndingEmailJob(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;
            Console.WriteLine("Task run: Sending Event Ending notice email!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event Ending notice email Job: {currentJob.Key}");
            //return Task.CompletedTask;
        }
    }
}
