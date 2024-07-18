using Event_Management.Application;
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.Service.FileService;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Entity;
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
        private readonly IUserService _userService;
        public EventController(IEventService eventService, IImageService fileService, IUserService userService)
        {
            _eventService = eventService;
            _fileService = fileService;
            _userService = userService;
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetEventInfo([FromQuery, Required] Guid eventId)
        {
            var response = await _eventService.GetEventInfo(eventId);
            if(response.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(response);
            }
            if(response.StatusResponse == HttpStatusCode.NotFound)
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }
        [Authorize]
        [HttpGet("user-event-role")]
        public async Task<IActionResult> GetEventByUserRole([FromQuery, Required] EventRole eventRole,
                                                        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            string userId = User.GetUserIdFromToken();
            var response = await _eventService.GetEventByUserRole(eventRole, userId, pageNo, elementEachPage);
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
        [HttpGet("tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByTag([FromQuery] List<int> TagId,
                                                        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
        {
            var response = await _eventService.GetEventsByListTag(TagId, pageNo, elementEachPage);
            if(response.TotalItems > 0)
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
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            });
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
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data= null
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
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            });
        }
        [Authorize]
        [HttpGet("user-past-and-incoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserPastAndIncomingEvent(/*[FromQuery, Required]Guid userId*/)
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
            string userId = User.GetUserIdFromToken();
            var result = await _eventService.AddEvent(eventDto, userId);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromBody] EventRequestDto eventDto, [FromQuery, Required] Guid eventId)
        {
            string userId = User.GetUserIdFromToken();
            var result = await _eventService.UpdateEvent(eventDto, userId.ToString(), eventId);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            if(result.StatusResponse == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteEvent([FromQuery, Required]Guid eventId)
        {
            string userId = User.GetUserIdFromToken();
            APIResponse response = new APIResponse();
            var result = await _eventService.DeleteEvent(eventId, Guid.Parse(userId));
            if (result)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.DeleteSuccessfully;
                response.Data = result;
                return Ok(response);
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.DeleteFailed;
                return BadRequest(response);
            }
            
        }

        [HttpPost("logo-upload")]
        public async Task<IActionResult> UploadEventLogoImage([FromBody] FileUploadDto dto)
        {
            var result = await _fileService.UploadEventSponsorLogo(dto.base64, dto.eventId, dto.sponsorName);
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
                Message = "Logo already exist!"
            });

        }
        [HttpGet("/logo")]
        public async Task<IActionResult> GetBlobUri([FromQuery] string brandName)
        {
            var result = await _fileService.GetBlobUri(brandName);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }
        [HttpGet("/logo/all")]
        public async Task<IActionResult> GetAllLogos()
        {
            var result = await _fileService.GetAllBlobUris();
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            });

        }
        [HttpGet("/logo/event-logo")]
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
            var result = _eventService.GetTop10CreatorsByEventCount();
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



        [Authorize]
        [HttpGet("event-statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<APIResponse> GetEventStatictis(Guid eventId)
        {
            string userId = User.GetUserIdFromToken();
            var response = new APIResponse();
            var isOwner = await _eventService.IsOwner(eventId, Guid.Parse(userId));

            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.Unauthorized;
                response.Message = MessageParticipant.NotOwner;
                response.Data = null;
                return response;
            }

            var result = await _eventService.GetEventStatis(eventId);
            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
            }
            return response;


        }
        [HttpGet("user-hosted")]
        public async Task<IActionResult> GetUserHostEvent([FromQuery, Required] Guid userId)
        {
            var result = await _eventService.GetUserHostEvent(userId);
            if(result != null)
            {
                return Ok(new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = result
                });
            }
            return NotFound(new APIResponse
                {
                StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                });
        }

    }
}




