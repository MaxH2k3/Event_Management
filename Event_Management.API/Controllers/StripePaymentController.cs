using Event_Management.Application;
using Event_Management.Application.Message;
using Event_Management.Application.Service;
using Event_Management.Domain.Models.Payment.StripePayment;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Net;

namespace Event_Management.API.Controllers
{

    [Route("api/stripepayment")]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private static string s_wasmClientURL = string.Empty;
        private readonly IEventService _eventService;

        public StripePaymentController(IConfiguration configuration, IEventService eventService)
        {
            _configuration = configuration;
            _eventService = eventService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<APIResponse> CheckoutOrder([FromBody] Guid eventId, [FromServices] IServiceProvider sp)
        {
            var response = new APIResponse();
            var referer = Request.Headers.Referer;
            s_wasmClientURL = referer[0];

            // Build the URL to which the customer will be redirected after paying.
            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }

            if (thisApiUrl is not null)
            {
                var sessionId = await CheckOut(eventId, thisApiUrl);
                var pubKey = _configuration["Stripe:PubKey"];

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = sessionId,
                    PubKey = pubKey
                };

                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = checkoutOrderResponse;



                return response;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.InternalServerError;
                response.Message = MessageCommon.NotComplete;
                return response;
            }
        }

        [NonAction]
        public async Task<string> CheckOut(Guid eventId, string thisApiUrl)
        {
            var eventEntity = await _eventService.GetEventById(eventId);
            // Create a payment flow from the items in the cart.
            // Gets sent to Stripe API.
            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = s_wasmClientURL + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
            {
                "card"
            },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)eventEntity.Ticket, // Price is in USD cents.
                        Currency = "VND",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = eventEntity.EventName,
                            Description = eventEntity.Description,
                            Images = new List<string> { eventEntity.Image }
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        [HttpGet("success")]
        // Automatic query parameter handling from ASP.NET.
        // Example URL: https://localhost:7051/checkout/success?sessionId=si_123123123123
        public ActionResult CheckoutSuccess(string sessionId)
        {

            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            

            // Here you can save order and customer details to your database.
            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Redirect(s_wasmClientURL + "success");
        }
    }
}
