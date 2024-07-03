using Event_Management.Application.Dto.EventDTO.SponsorDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
	[Route("api/v1/sponsor")]
	[ApiController]
	public class SponsorController : ControllerBase
	{
		private readonly ISponsorEventService _sponsorEventService;

		public SponsorController(ISponsorEventService sponsorEventService)
		{
			_sponsorEventService = sponsorEventService;
		}

		//[Authorize]
		[HttpPost("request")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
		public async Task<APIResponse> CreateRequest([FromQuery] SponsorDto sponsorEvent)
		{
			APIResponse response = new APIResponse();
			//string userId = User.GetUserIdFromToken();
			//sponsorEvent.UserId = Guid.Parse(userId);
			var result = await _sponsorEventService.AddSponsorEventRequest(sponsorEvent);
			if(result)
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


		[HttpPut("request")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
		public async Task<APIResponse> UpdateRequest([FromQuery] SponsorDto sponsorEvent)
		{
			APIResponse response = new APIResponse();
			//string userId = User.GetUserIdFromToken();
			//sponsorEvent.UserId = Guid.Parse(userId);
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
	}
}
