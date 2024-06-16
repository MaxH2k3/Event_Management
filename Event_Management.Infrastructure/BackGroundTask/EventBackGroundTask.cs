using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.BackGroundTask
{
    public class EventBackGroundTask : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventBackGroundTask> _logger;
        public EventBackGroundTask(IUnitOfWork unitOfWork, ILogger<EventBackGroundTask> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            //double demo =  _unitOfWork.EventRepository.UpdateEventStatusToEnded();
            //double demo2 =  _unitOfWork.EventRepository.UpdateEventStatusToOnGoing();
            _logger.LogInformation("Task run!" /*+ demo2 + " " + demo*/);
            return Task.CompletedTask;
        }
    }
}
