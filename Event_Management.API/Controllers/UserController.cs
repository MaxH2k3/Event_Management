using System.ComponentModel.DataAnnotations;
using System.Net;
using Event_Management.Application.Service;
using Event_Management.Application.Message;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Mvc;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Domain.Helper;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserById()
    {
        var userId = User.GetUserIdFromToken();
        var result = await _userService.GetUserByIdAsync(Guid.Parse(userId));
        return Ok(result);
    }

    [Authorize]
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

    [Authorize]
    [HttpGet("keyword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByKeyword([FromQuery] string keyword)
    {
        var result = await _userService.GetByKeyWord(keyword);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return BadRequest(result.StatusResponse);
        }
        return Ok(result);
    }

    [Authorize]
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

    [Authorize("Admin")]
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromQuery] Guid userId)
    {
        var result = await _userService.DeleteUser(userId);
        if (result.StatusResponse != HttpStatusCode.OK)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

}
