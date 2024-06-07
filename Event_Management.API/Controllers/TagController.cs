﻿using Event_Management.Application.Dto;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        private readonly IWebHostEnvironment _environment;

        public TagController(ITagService tagService, IWebHostEnvironment environment)
        {
            _tagService = tagService;
            _environment = environment;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllTag([FromQuery, Range(1, int.MaxValue)] int page = 1,
                                                                   [FromQuery, Range(1, int.MaxValue)] int eachPage = 10)
        {
            APIResponse response = new APIResponse();
            var result = await _tagService.GetAllTag(page, eachPage);


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
        public async Task<APIResponse> AddTag([FromBody] TagDTO tagDTO)
        {
            APIResponse response = new APIResponse();
            var result = await _tagService.AddTag(tagDTO);

            if (result)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.SavingSuccesfully;
                response.Data = result;
                
            } else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.SavingFailed;
            }
            return response;
           
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<APIResponse> UpdateTag([FromBody] TagDTO tagDTO)
        {
            APIResponse response = new APIResponse();
            var result = await _tagService.UpdateTag(tagDTO);
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
        public async Task<APIResponse> DeleteTag(int TagId)
        {
            APIResponse response = new APIResponse();
            var result = await _tagService.DeleteTag(TagId);
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