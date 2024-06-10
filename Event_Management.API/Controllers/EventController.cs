﻿using Event_Management.Application;
using Event_Management.Application.Service;
using Event_Management.Domain.Message;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management.API.Controllers
{
	[Route("api/v1/events")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

		public EventController(IEventService eventService)
        {
            _eventService = eventService;
		}

        [Authorize]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllEvents([FromQuery] EventFilterObject filterObject, 
                                                      [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, 
                                                      [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = await _eventService.GetAllEvents(filterObject, pageNo, elementEachPage);
            if(response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "Get all events!",
                    Data = response
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = "Not found!",
            });
        }
        [HttpGet("UserParticipated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserParticipatedEvents([FromQuery] EventFilterObject filter, [FromQuery, Required] string userId, 
                                                                   [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, 
                                                                   [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = await _eventService.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            if(response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "Get all user participated events!",
                    Data = response
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = "Not found!",
            });
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> AddEvent(EventRequestDto eventDto)
        {
            APIResponse response = new APIResponse();
            var result = await _eventService.AddEvent(eventDto);

            if (result)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.SavingSuccesfully;
                response.Data = result;

            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.SavingFailed;
            }
            return response;
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<APIResponse> UpdateEvent([FromBody] EventRequestDto eventDto)
        {
            APIResponse response = new APIResponse();
            var result = await _eventService.UpdateEvent(eventDto);
            if (result)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.UpdateSuccesfully;
                response.Data = result;

            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.UpdateFailed;
            }
            return response;
        }

        [HttpDelete("")]
        public async Task<APIResponse> DeleteEvent(Guid packageId)
        {
            APIResponse response = new APIResponse();
            var result = await _eventService.DeleteEvent(packageId);
            if (result)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.DeleteSuccessfully;
                response.Data = result;

            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.DeleteFailed;
            }
            return response;
        }
    }
  //      [HttpGet("GetTest")]
		//public async Task<IActionResult> GetTest()
  //      {
  //          var response = await _eventService.GetAllEventTest();

  //          return Ok(response);
		//}

}

