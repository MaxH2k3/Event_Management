using System.ComponentModel.DataAnnotations;
using System.Net;
using Event_Management.Application.Service;
using Event_Management.Application.Message;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;
    

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<APIResponse> GetAllUsers([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10)
    {
        var result = await _userService.GetAllUser(pageNo, eachPage);
        if (result.Any())
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result,
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.Complete,
            Data = result,
        };
    }

}
