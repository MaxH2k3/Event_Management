using Event_Management.Application.Helper;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/v1/participants")]
    public class ParticipantController : Controller
    {
        private readonly IRegisterEventService _registerEventService;
        private readonly IEventService _eventService;

        public ParticipantController(IRegisterEventService registerEventService,
            IEventService eventService)
        {
            _registerEventService = registerEventService;
            _eventService = eventService;
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetParticipantOnEvent([FromQuery] FilterParticipant filter)
        {
            var isOwner = await _eventService.IsOwner(filter.EventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            var response = await _registerEventService.GetParticipantOnEvent(filter);

            if (response.Any())
            {
                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());

                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> RegisterEvent(Guid eventId)
        {
            var registerEventModel = new RegisterEventModel
            {
                UserId = Guid.Parse(User.GetUserIdFromToken()),
                EventId = eventId,
                RoleEventId = (int)EventRole.Visitor
            };

            var response = await _registerEventService.RegisterEvent(registerEventModel);

            if (response.StatusResponse == HttpStatusCode.Created)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddParticipantToEvent(RegisterEventModel registerEventModel)
        {
            var isOwner = await _eventService.IsOwner(registerEventModel.EventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            var response = await _registerEventService.AddToEvent(registerEventModel);

            if (response.StatusResponse == HttpStatusCode.Created)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("role")]
        [Authorize]
        public async Task<IActionResult> UpdateRoleEvent(RegisterEventModel registerEventModel)
        {
            var isOwner = await _eventService.IsOwner(registerEventModel.EventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            var response = await _registerEventService.UpdateRoleEvent(registerEventModel);

            if (response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("")]
        [Authorize]
        public async Task<IActionResult> DeleteParticipant(Guid userId, Guid eventId)
        {
            var isOwner = await _eventService.IsOwner(eventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            var response = await _registerEventService.DeleteParticipant(userId, eventId);

            if (response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);

        }

        [HttpGet("qrcode")]
        public IActionResult QRCode(string data)
        {
            var bytes = QRCodeHelper.GenerateQRCode(data);

            return File(bytes, "image/png");

        }

        [HttpPost("accept-ticket")]
        [Authorize]
        public async Task<IActionResult> AcceptParticipant(RegisterEventModel registerEventModel)
        {
            var isOwner = await _eventService.IsOwner(registerEventModel.EventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            var response = await _registerEventService.AcceptRegisterEvent(registerEventModel.EventId, registerEventModel.UserId);

            if (response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("approval/{eventId}")]
        [Authorize]
        public async Task<IActionResult> ApprovalInvite(Guid eventId)
        {
            var userId = Guid.Parse(User.GetUserIdFromToken());
            var response = await _registerEventService.AcceptRegisterEvent(eventId, userId);

            if (response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
