using Event_Management.Application.Service.Job;
using Event_Management.Application.ServiceTask;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/v1/system")]
    public class SystemController : Controller
    {
        private readonly IQuartzService _quartzService;
        private readonly ISendMailTask _sendMailTask;

        public SystemController(IQuartzService quartzService, ISendMailTask sendMailTask)
        {
            _quartzService = quartzService;
            _sendMailTask = sendMailTask;
        }

        [HttpGet("jobs")]
        public IActionResult GetAllJob()
        {
            return Ok(_quartzService.GetAllJob());
        }

        [HttpPost("jobs")]
        public IActionResult ExcuteJob(string key, int type)
        {
            _quartzService.ExcuteJob(key, type);
            return Ok();
        }

        [HttpGet("send")]
        public IActionResult TestEmail()
        {
            _sendMailTask.SendMailReminder(Guid.Parse("14A7C72E-706B-47F3-ADFC-0A6E80898C04"));

            return Ok("System API");
        }
    }
}
