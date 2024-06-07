using Event_Management.Application.Dto;
using Event_Management.Application.Dto.PackageDto;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.Service.PackageEvent;
using Event_Management.Application.Service.TagEvent;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/package")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        private readonly IWebHostEnvironment _environment;

        public PackageController(IPackageService packageService, IWebHostEnvironment environment)
        {
            _packageService = packageService;
            _environment = environment;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllPackage([FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
        {
            APIResponse response = new APIResponse();
            var result = await _packageService.GetAllPackage(page, eachPage);


            if (result.Any())
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.NotFound;
            }
            return response;

        }

        [HttpGet("eventId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetPackageByEventId([FromQuery] Guid eventId, [FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
        {
            APIResponse response = new APIResponse();
            var result = await _packageService.GetPackageByEventId(eventId, page, eachPage);


            if (result.Any())
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.NotFound;
            }
            return response;

        }

        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> AddPackage(PackageDto packageDto)
        {
            
            APIResponse response = new APIResponse();
            var result = await _packageService.AddPackage(packageDto);

            if (result)
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
        public async Task<APIResponse> UpdatePackage([FromBody] PackageDto packageDto)
        {
            APIResponse response = new APIResponse();
            var result = await _packageService.UpdatePackage(packageDto);
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

        [HttpDelete("")]
        public async Task<APIResponse> DeletePackage(Guid packageId)
        {
            APIResponse response = new APIResponse();
            var result = await _packageService.DeletePackage(packageId);
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

    }
}
