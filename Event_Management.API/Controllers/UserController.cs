using System.ComponentModel.DataAnnotations;
using System.Net;
using Event_Management.Application.Service;
using Event_Management.Application.Message;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Mvc;
using Event_Management.Application.Dto.UserDTO.Request;

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
    public async Task<IActionResult> GetAllUsers([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10)
    {
        var result = await _userService.GetAllUser(pageNo, eachPage);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return BadRequest(result.StatusResponse);
        }
        return Ok(result);
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateDeleteUserDto userDto)
    {
        var result = await _userService.UpdateUser(userDto);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(UpdateDeleteUserDto userDto)
    {
        var result = await _userService.DeleteUser(userDto);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

}
