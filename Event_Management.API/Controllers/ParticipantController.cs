using Event_Management.Application.Dto.ParticipantDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
	[ApiController]
	[Route("api/v1/participants")]
	public class ParticipantController : Controller
	{
		private readonly IRegisterEventService _registerEventService;

		public ParticipantController(IRegisterEventService registerEventService)
		{
			_registerEventService = registerEventService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterEvent([FromForm] RegisterEventModel registerEventModel)
		{

			var response = await _registerEventService.RegisterEvent(registerEventModel);

			if (response.StatusResponse == HttpStatusCode.Created)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPut("role")]
		public async Task<IActionResult> UpdateRoleEvent([FromForm] RegisterEventModel registerEventModel)
		{
			var response = await _registerEventService.UpdateRoleEvent(registerEventModel);

			if (response.StatusResponse == HttpStatusCode.OK)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpDelete("")]
		public async Task<IActionResult> DeleteParticipant(Guid userId, Guid eventId)
		{
			var response = await _registerEventService.DeleteParticipant(userId, eventId);

			if (response.StatusResponse == HttpStatusCode.OK)
			{
				return Ok(response);
			}

			return BadRequest(response);

		}


	}
}
