using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service.Payments.PayPalService;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/paypal")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _payPalService;


        public PayPalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost("payment")]
        public async Task<APIResponse> CreatePayment(Guid eventId, string description)
        {
            APIResponse response = new APIResponse();

            var result = await _payPalService.CreatePayment(eventId, description);

            if(result != null)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.CreateSuccesfully;
                response.Data = result;
            }
            return response;

            

        }


        [HttpPost("payout")]
        public async Task<APIResponse> CreatePayout(Guid eventId)
        {
            APIResponse response = new APIResponse();

            var result = await _payPalService.CreatePayout(eventId);

            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.CreateSuccesfully;
                response.Data = result;
            }
            return response;
        }
    }
}
