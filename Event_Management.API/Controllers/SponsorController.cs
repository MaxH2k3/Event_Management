using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.WebSockets;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/sponsor")]
	[ApiController]
	public class SponsorController : ControllerBase
	{
		private readonly ISponsorEventService _sponsorEventService;
		private readonly IUserService _userService;
		private readonly IEventService _eventService;

		public SponsorController(ISponsorEventService sponsorEventService, IUserService userService, IEventService eventService)
		{
			_sponsorEventService = sponsorEventService;
			_userService = userService;
			_eventService = eventService;
		}

		[Authorize]
		[HttpPost("request")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
		public async Task<APIResponse> CreateRequest([FromBody] SponsorDto sponsorEvent)
		{
			APIResponse response = new APIResponse();
			var userId = Guid.Parse(User.GetUserIdFromToken());
			var userEntity = await _userService.GetUserByIdAsync(userId);
			if (userEntity == null)
			{
				response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message = MessageUser.UserNotFound;
				response.Data = null;
				return response;
			}

			var result = await _sponsorEventService.AddSponsorEventRequest(sponsorEvent, userId);
			if(result != null)
			{
				response.StatusResponse = HttpStatusCode.Created;
				response.Message = MessageCommon.CreateSuccesfully;
				response.Data = result;
			} else
			{
				response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message = MessageCommon.CreateFailed;
			}
			
			return response;
		}

        [Authorize]
        [HttpPut("request-status")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
		public async Task<APIResponse> UpdateRequest([FromBody] SponsorRequestUpdate sponsorRequestUpdate)
		{
			APIResponse response = new APIResponse();
			var userId = Guid.Parse(User.GetUserIdFromToken());
            var isOwner = await _eventService.IsOwner(sponsorRequestUpdate.EventId, userId);

            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageParticipant.NotOwner;
                response.Data = null;
                return response;
            }

            
			var result = await _sponsorEventService.UpdateSponsorEventRequest(sponsorRequestUpdate.EventId, userId, sponsorRequestUpdate.Status);
			if (result != null)
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


        [Authorize]
        [HttpGet("event-filter")]
        //Get requested-sponsor of this event
        public async Task<APIResponse> GetSponsorEvent([FromQuery] SponsorEventFilter sponsorFilter)
        {
            var response = new APIResponse();
			var isOwner = await _eventService.IsOwner(sponsorFilter.EventId, Guid.Parse(User.GetUserIdFromToken()));
			if (!isOwner)
			{
				response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message = MessageParticipant.NotOwner;
				response.Data = null;
                return response;
			}

			var result = await _sponsorEventService.GetSponsorEventsById(sponsorFilter);
            if (result.Count() > 0)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
                response.Data = result;
            }

            return response;

        }


        [Authorize]
        [HttpGet("requested-sponsor")]
        //Get requested-sponsor of this person
        public async Task<APIResponse> GetRequestSponsor(string? status, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = new APIResponse();

            
            var result = await _sponsorEventService.GetRequestSponsor(Guid.Parse(User.GetUserIdFromToken()), status, pageNo, elementEachPage);
            if (result.Count() > 0)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
                response.Data = result;
            }

            return response;

        }

		[Authorize]
		[HttpDelete("request")]
		public async Task<APIResponse> DeleteRequest(Guid eventId)
		{
            APIResponse response = new APIResponse();
            var userId = Guid.Parse(User.GetUserIdFromToken());
            var userEntity = await _userService.GetUserByIdAsync(userId);
            if (userEntity == null)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageUser.UserNotFound;
                response.Data = null;
                return response;
            }

			var result = await _sponsorEventService.DeleteRequest(eventId, userId);

            if (result != null)
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
}
