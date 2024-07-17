using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/transaction")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _transactionService;
        private readonly IUserService _userService;

        public PaymentTransactionController(IPaymentTransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }


        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> CreateTransaction([FromBody] TransactionRequestDto transactionRequestDto)
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

            var result = await _transactionService.AddTransaction(transactionRequestDto);
            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.CreateSuccesfully;
                response.Data = result;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.CreateFailed;
            }

            return response;
        }


        [Authorize("Admin")]
        [HttpGet("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<APIResponse> GetAllTransaction([FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
                                                        [FromQuery, Range(1, int.MaxValue)] int elementEachPage = 10)
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

            var result = await _transactionService.GetAllTransaction(pageNo, elementEachPage);
            if (result.Count() > 0)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = result;
            }
            else { 
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
                response.Data = result;
            }

            return response;
        }




    }
}
