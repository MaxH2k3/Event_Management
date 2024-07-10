using Event_Management.Application.Dto;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service.FeedbackEvent;
using Event_Management.Domain;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Service.TagEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public FeedbackController(IFeedbackService feedbackService, IWebHostEnvironment environment)
        {
            _feedbackService = feedbackService;
            _environment = environment;
        }


        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> CreateFeedback([FromBody] FeedbackDto feedbackDto)
        {
            APIResponse response = new APIResponse();
            string userId = User.GetUserIdFromToken();
            if (userId.Equals(""))
            {
                response.StatusResponse = HttpStatusCode.Unauthorized;
                response.Message = MessageCommon.Unauthorized;
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

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<APIResponse> UpdateFeedback([FromBody] FeedbackDto feedbackDto)
        {
            APIResponse response = new APIResponse();
            string userId = User.GetUserIdFromToken();
            if (userId.Equals(""))
            {
                response.StatusResponse = HttpStatusCode.Unauthorized;
                response.Message = MessageCommon.Unauthorized;
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
        //[Authorize]
        [HttpGet("event")]
        public async Task<IActionResult> GetEventFeedbacks([FromQuery, Required] Guid eventId)
        {
            var result = await _feedbackService.GetEventFeedbacks(eventId);
            return Ok(result);
        }
    }
}
