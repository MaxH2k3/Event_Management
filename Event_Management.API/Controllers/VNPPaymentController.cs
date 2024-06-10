using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Service.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers
{
    [Route("api/vnp")]
    [ApiController]
    public class VNPPaymentController : ControllerBase
    {
        private readonly IVNPAYService _vnpayService;

        public VNPPaymentController(IVNPAYService vnpayService)
        {
            _vnpayService = vnpayService;
        }

        [HttpPost("create")]
        public IActionResult CreatePayment([FromBody] VNPAYPaymentRequest request)
        {
            var paymentUrl = _vnpayService.CreatePaymentUrl(request);
            return Ok(new { paymentUrl });
        }

        [HttpGet("return")]
        public IActionResult PaymentReturn()
        {
            if (_vnpayService.VerifyPayment(Request))
            {
                return Ok("Payment successful");
            }
            return BadRequest("Payment verification failed");
        }
    }
}
