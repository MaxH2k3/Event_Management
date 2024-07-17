using Event_Management.Application.Service.FileService;
using Event_Management.Application.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Event_Management.Application.Message;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    [Authorize("Admin")]
    public class DashBoardController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IImageService _fileService;
        private readonly IUserService _userService;
        public DashBoardController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        [HttpGet("users/total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalCountableUser()
        {
            var response = await _userService.GetTotalUser();
            return Ok(response);
        }

        [HttpGet("users/monthly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalUserByYear([FromQuery, Required]int year)
        {
            var response = await _userService.GetTotalUserByYear(year);
            return Ok(response);
        }

        [HttpGet("event/status")]
        public async Task<IActionResult> CountByStatus()
        {
            APIResponse response = new APIResponse();
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.Complete;
            response.Data = await _eventService.CountByStatus();
            return Ok(response);
        }

        [HttpGet("event/monthly")]
        public async Task<IActionResult> CountEventsPerMonth([FromQuery, Required] DateTime startDate, [FromQuery, Required] DateTime endDate)
        {
            APIResponse response = new APIResponse();
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.Complete;
            response.Data = await _eventService.EventsPerMonth(startDate, endDate);
            return Ok(response);
        }
    }
}
