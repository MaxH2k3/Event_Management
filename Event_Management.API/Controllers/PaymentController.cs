using Event_Management.API.Utilities;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Application.Service.Payments.VnpayService;
using Event_Management.Domain.Model.VnpayPayment;
using Event_Management.Domain.Models.System;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly VnpaySetting _vnpaySetting;
     

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="vnpayConfigOptions"></param>
        public PaymentController(IMediator mediator,
            IOptions<VnpaySetting> vnpayConfigOptions)
        {
           _mediator = mediator;
      
           _vnpaySetting = vnpayConfigOptions.Value;
        }

        /// <summary>
        /// Create payment to get link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(PaymentLinkDto), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] PaymentDto request)
        {
           
            var response = new PaymentLinkDto();
            response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayPayResponse vnpayresponse)
        {
            //var response = new APIResponse();
            string returnUrl = string.Empty;
            var returnModel = new PaymentReturnDto();
            var vnpaySetting = new VnpaySetting();
            var processResult = await _mediator.Send(vnpayresponse.Adapt<ProcessVnpayPaymentReturn>());

            //if(processResult != null)
            //{
            //    response.StatusResponse = HttpStatusCode.OK;
            //    response.Message = MessageCommon.Complete;
            //    response.Data = processResult.PaymentStatus;
            //} else
            //{
            //    response.StatusResponse = HttpStatusCode.BadRequest;
            //    response.Message = MessageCommon.NotComplete;
            //}

            //return response;

            if(processResult != null)
            {
                returnModel = processResult;
                returnUrl = vnpaySetting.ReturnUrl;
            }
            if (returnUrl.EndsWith("/"))
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");


        }

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
