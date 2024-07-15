using Event_Management.Application.Dto.EventDTO.SponsorDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
		public async Task<APIResponse> CreateRequest([FromQuery] SponsorDto sponsorEvent)
		{
			APIResponse response = new APIResponse();
			string userId = User.GetUserIdFromToken();
			var userEntity = await _userService.GetUserByIdAsync(Guid.Parse(userId));
			if(userEntity == null)
			{
                response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message =  MessageUser.UserNotFound;
				response.Data = null;
				return response;
            }
			sponsorEvent.UserId = Guid.Parse(userId);
			var result = await _sponsorEventService.AddSponsorEventRequest(sponsorEvent);
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
        [HttpPut("request")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
		public async Task<APIResponse> UpdateRequest([FromQuery] SponsorDto sponsorEvent)
		{
			APIResponse response = new APIResponse();
			string userId = User.GetUserIdFromToken();
            var isOwner = await _eventService.IsOwner(sponsorEvent.EventId, Guid.Parse(userId));

            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageParticipant.NotOwner;
                response.Data = null;
                return response;
            }

            sponsorEvent.UserId = Guid.Parse(userId);
			var result = await _sponsorEventService.UpdateSponsorEventRequest(sponsorEvent);
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
        [HttpGet("event")]
        public async Task<APIResponse> GetSponsorEvent([FromQuery] SponsorEventFilter sponsorFilter)
        {
            var response = new APIResponse();
			var isOwner = await _eventService.IsOwner(sponsorFilter.EventId, Guid.Parse(User.GetUserIdFromToken()));
			if (!isOwner)
			{
				response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message = MessageParticipant.NotOwner;
				response.Data = null;
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
        [HttpGet("sponsored-event")]
        public async Task<APIResponse> GetSponsoredEvent([FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = new APIResponse();

            
            var result = await _sponsorEventService.GetSponsoredEvent(Guid.Parse(User.GetUserIdFromToken()), pageNo, elementEachPage);
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

    }
}
