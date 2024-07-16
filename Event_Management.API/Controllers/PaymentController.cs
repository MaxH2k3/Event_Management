using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.Service.Payments.PayPalService;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        //private readonly VnpaySetting _vnpaySetting;
        private readonly IEventService _eventService;
        private readonly IPayPalService _payPalService;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="vnpayConfigOptions"></param>
        public PaymentController(IMediator mediator, IEventService eventService, IPayPalService payPalService)
        //IOptions<VnpaySetting> vnpayConfigOptions
        {
           _mediator = mediator;
           _eventService = eventService;
            //_vnpaySetting = vnpayConfigOptions.Value;
            _payPalService = payPalService;
        }

        [Authorize]
        [HttpPost("paypal")]
        public async Task<APIResponse> CreatePayment(Guid eventId, string description)
        {
            APIResponse response = new APIResponse();
			var userId = Guid.Parse(User.GetUserIdFromToken());
			var result = await _payPalService.CreatePayment(eventId, userId, description);

            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.CreateSuccesfully;
                response.Data = result;
            } else
            {
				response.StatusResponse = HttpStatusCode.BadRequest;
				response.Message = MessageCommon.CreateFailed;
			}
            return response;



        }

        [Authorize]
        [HttpPost("payout")]
        public async Task<APIResponse> CreatePayout(Guid eventId, string emailReceiver, decimal amount)
        {
            APIResponse response = new APIResponse();
            var isOwner = await _eventService.IsOwner(eventId, Guid.Parse(User.GetUserIdFromToken()));
            if (!isOwner)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageParticipant.NotOwner;
                response.Data = null;
            }
            var result = await _payPalService.CreatePayout(eventId, emailReceiver, amount);

            if (result != null)
            {
                response.StatusResponse = HttpStatusCode.Created;
                response.Message = MessageCommon.CreateSuccesfully;
                response.Data = result;
            }
            return response;
        }

        /// <summary>
        /// Create payment to get link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost("")]
        //[ProducesResponseType(typeof(PaymentLinkDto), 200)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> Create([FromBody] PaymentDto request)
        //{

        //    string userId = User.GetUserIdFromToken();
        //    request.CreatedBy = Guid.Parse(userId)
        //        ;
        //    var response = new PaymentLinkDto();
        //    response = await _mediator.Send(request);
        //    return Ok(response);
        //}

        //[HttpGet]
        //[Route("vnpay-return")]
        //public async Task<APIResponse> VnpayReturn([FromQuery] VnpayPayResponse vnpayresponse)
        //{
        //    var response = new APIResponse();
        //    //string returnUrl = string.Empty;
        //    //var returnModel = new PaymentReturnDto();
        //    var vnpaySetting = new VnpaySetting();
        //    var processResult = await _mediator.Send(vnpayresponse.Adapt<ProcessVnpayPaymentReturn>());

        //    //if(processResult != null)
        //    //{
        //    //    response.StatusResponse = HttpStatusCode.OK;
        //    //    response.Message = MessageCommon.Complete;
        //    //    response.Data = processResult.PaymentStatus;
        //    //} else
        //    //{
        //    //    response.StatusResponse = HttpStatusCode.BadRequest;
        //    //    response.Message = MessageCommon.NotComplete;
        //    //}

        //    //return response;

        //    if(processResult != null)
        //    {

        //        //returnUrl = vnpaySetting.ReturnUrl as string;
        //        response.StatusResponse = HttpStatusCode.OK;
        //        response.Message = MessageCommon.Complete;
        //        response.Data = processResult as PaymentReturnDto;
        //    }
        //    else
        //    {
        //        response.StatusResponse = HttpStatusCode.BadRequest;
        //        response.Message = MessageCommon.NotComplete;
        //        response.Data = processResult as PaymentReturnDto;
        //    }

        //    return response;
        //    //if (!string.IsNullOrEmpty(returnUrl) && returnUrl.EndsWith("/"))
        //    //    returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
        //    ////Console.WriteLine($"{returnUrl}?{returnModel.ToQueryString()}");
        //    //return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");


        //}

        //[HttpGet]
        //[Route("momo-return")]
        //public async Task<IActionResult> MomoReturn([FromQuery] MomoOneTimePaymentResultRequest response)
        //{
        //    string returnUrl = string.Empty;
        //    var returnModel = new PaymentReturnDtos();
        //    var processResult = await mediator.Send(response.Adapt<ProcessMomoPaymentReturn>());

        //    if (processResult.Success)
        //    {
        //        returnModel = processResult.Data.Item1 as PaymentReturnDtos;
        //        returnUrl = processResult.Data.Item2 as string;
        //    }

        //    if (returnUrl.EndsWith("/"))
        //        returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
        //    return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");
        //}
    }
}
