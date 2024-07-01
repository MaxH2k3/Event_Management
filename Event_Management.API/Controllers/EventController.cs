using Event_Management.Application;
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.Service.FileService;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IImageService _fileService;
        public EventController(IEventService eventService, IImageService fileService)
        {
            _eventService = eventService;
            _fileService = fileService;
        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllEvents([FromQuery] EventFilterObject filterObject,
                                                      [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                      [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = await _eventService.GetAllEvents(filterObject, pageNo, elementEachPage);
            if (response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageEvent.GetAllEvent,
                    Data = response
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
            });
        }

        [Authorize]
        [HttpGet("user-participated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserParticipatedEvents([FromQuery] EventFilterObject filter,
                                                                   [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            string userId = User.GetUserIdFromToken();
            var response = await _eventService.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            if (response.TotalItems > 0)
            {

                Response.Headers.Add("X-Total-Element", response.TotalItems.ToString());
                Response.Headers.Add("X-Total-Page", response.TotalPages.ToString());
                Response.Headers.Add("X-Current-Page", response.CurrentPage.ToString());
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageEvent.UserParticipatedEvent,
                    Data = response
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
            });
        }
        [Authorize]
        [HttpGet("user-past-and-incoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserPastAndIncomingEvent()
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var response = await _eventService.GetUserPastAndFutureEvents(userId);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response
            });
        }
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEvent(EventRequestDto eventDto)
        {
            APIResponse response = new APIResponse();

            string userId = User.GetUserIdFromToken();
            var result = await _eventService.AddEvent(eventDto, userId);
            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Data = result;
                response.Message = MessageCommon.Complete;
                return Ok(response);
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed
            });
        }

        [Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> UpdateEvent([FromBody] EventRequestDto eventDto)
        {
            APIResponse response = new APIResponse();
            string userId = User.GetUserIdFromToken();
            var result = await _eventService.UpdateEvent(eventDto, userId);
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

        [Authorize]
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
        [HttpPost("file-upload")]
        public async Task<IActionResult> UploadEventImage([FromBody] FileUploadDto dto)
        {
            var result = await _fileService.UploadImage2(dto);
            if (result != null)
            {
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = result
                });
            }
            return BadRequest(new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = "Error while create Event Image!"
            });

        }
        [HttpGet("/image")]
        public async Task<IActionResult> GetBlobUri([FromQuery] string blobName)
        {
            var result = await _fileService.GetBlobUri(blobName);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }
        [HttpGet("/image/all")]
        public async Task<IActionResult> GetAllBlobUri()
        {
            var result = await _fileService.GetAllBlobUris();
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }
        [HttpGet("/image/event-image")]
        public async Task<IActionResult> GetAllEventBlobUri([FromQuery] Guid eventId)
        {
            var result = await _fileService.GetAllEventBlobUris(eventId);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }
        [HttpGet("popular/organizers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PopularOrganizers()
        {
            var result = _eventService.GetEventLeaderBoards();
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.PopularOrganizers,
                Data = result
            });
        }
        [HttpGet("popular/locations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Locations()
        {
            var result = _eventService.GetTop10LocationByEventCount();
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.PopularLocation,
                Data = result
            });
        }

    }
}

//      [HttpGet("GetTest")]
//public async Task<IActionResult> GetTest()
//      {
//          var response = await _eventService.GetAllEventTest();

//          return Ok(response);
//}



