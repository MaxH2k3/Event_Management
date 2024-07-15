using Event_Management.API.Utilities;
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
using System.ComponentModel.DataAnnotations;
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

            Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());

            return Ok(response);
        }

        [HttpGet("checkin")]
        [Authorize]
        public async Task<IActionResult> GetParticipantRelatedToCheckInOnEvent([Required] Guid eventId, int page = 1, int eachPage = 10)
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

            var response = await _registerEventService.GetParticipantOnEvent(page, eachPage, eventId);

            Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());

            return Ok(response);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> RegisterEvent(Guid eventId, Guid? transactionId)
        {
            if (transactionId != null && !Guid.TryParse(transactionId.ToString(), out _))
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.TransactionIsNotValid,
                    Data = null
                });
            }

            var isOwner = await _eventService.IsOwner(eventId, Guid.Parse(User.GetUserIdFromToken()));

            if (isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.HostCannotRegister,
                    Data = null
                });
            }

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

            isOwner = await _eventService.IsOwner(registerEventModel.EventId, registerEventModel.UserId);

            if (isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.HostCannotRegister,
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
        public IActionResult QRCode(Guid eventId, Guid userId)
        {
            var bytes = QRCodeHelper.GenerateQRCode(userId.ToString());

            return File(bytes, "image/png");

        }

        [HttpPost("process-ticket")]
        [Authorize]
        public async Task<IActionResult> ProcessTicketParticipant(ParticipantTicket participantTicket)
        {
            var isOwner = await _eventService.IsOwner(participantTicket.EventId, Guid.Parse(User.GetUserIdFromToken()));

            if (!isOwner)
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                });
            }

            if (!Helper.IsValidParticipantStatus(participantTicket.Status))
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.ParticipantStatusNotValid,
                    Data = null
                });
            }

            var response = await _registerEventService.ProcessingTicket(participantTicket.EventId, participantTicket.UserId, participantTicket.Status);

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
            var response = await _registerEventService.ProcessingTicket(eventId, userId, ParticipantStatus.Confirmed.ToString());

            if (response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetUserRegisterStatus([Required] Guid eventId)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var response = await _registerEventService.GetCurrentUser(userId, eventId);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("checkin-user")]
        public async Task<IActionResult> CheckinParticipant(Guid eventId, Guid userId)
        {
            if(await _registerEventService.IsRole(userId, eventId, EventRole.CheckingStaff))
            {
                return BadRequest(new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.YouAreNotStaff,
                    Data = null
                });
            }

            var response = await _registerEventService.CheckInParticipant(userId, eventId);
            return Ok(response);
        }

    }
}
