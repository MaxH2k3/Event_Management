using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        private readonly IWebHostEnvironment _environment;
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public FeedbackController(IFeedbackService feedbackService, IWebHostEnvironment environment, IEventService eventService, IUserService userService)
        {
            _feedbackService = feedbackService;
            _environment = environment;
            _eventService = eventService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> CreateFeedback([FromBody] FeedbackDto feedbackDto)
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
            //string userId = feedbackDto.UserId.ToString();
            var result = await _feedbackService.AddFeedback(feedbackDto, userId);

            if (result != null)
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

        [Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<APIResponse> UpdateFeedback([FromBody] FeedbackDto feedbackDto)
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
            //string userId = feedbackDto.UserId.ToString();
            var result = await _feedbackService.UpdateFeedback(feedbackDto, userId);
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

        //Get all feedback for my event
        public async Task<APIResponse> GetEventFeedbacks([FromQuery, Required] Guid eventId, [FromQuery, Range(1, 5)]  int? numOfStar, [FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
        {
            var response = new APIResponse();
            var isOwner = await _eventService.IsOwner(eventId, Guid.Parse(User.GetUserIdFromToken()));
            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageParticipant.NotOwner;
                response.Data = null;
            }

            var result = await _feedbackService.GetEventFeedbacks(eventId, numOfStar, page, eachPage);
            if(result != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            } else
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
                response.Data = result;
            }
           
            return response;
            
        }
        [Authorize]
        [HttpGet("event/user")]
        //Gte feedback detail for specific event
        public async Task<IActionResult> GetUserFeedBack([FromQuery, Required] Guid eventId)
        {
            string? userId = User.GetUserIdFromToken();
            var result = await _feedbackService.GetUserFeedback(eventId, Guid.Parse(userId));

            if(result != null)
            {
                APIResponse response = new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = result
                };
                return Ok(response);
            }
            return NotFound(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
            });
        }
        [Authorize]
        [HttpGet("user")]
        //Get all feedback of this user for all events
        public async Task<IActionResult> GetAllUserFeebacks([FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
        {
            string? userId = User.GetUserIdFromToken();
            var result = await _feedbackService.GetAllUserFeebacks(Guid.Parse(userId), page,eachPage);
            APIResponse response = new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = result
            };
            return Ok(response);
        }
    }
}
