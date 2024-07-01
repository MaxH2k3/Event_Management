using Azure;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Dto.PaymentDTO.PayPalPayment;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System.Text.Json;

namespace Event_Management.Application.Service.Payments.PayPalService
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        public PayPalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }
        public async Task<PayPal.Api.Payment> CreatePayment(Guid eventId, string description)
        {
            
            var apiContext = GetApiContext();
            string baseUrl = "https://localhost:7153";
            var eventEtity = await _unitOfWork.EventRepository.GetById(eventId);

            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
            {
                new Transaction
                {
                    description = description,
                    amount = new Amount
                    {
                        currency = "USD",
                        total = eventEtity.Fare.ToString()
                    }
                }
            },
                redirect_urls = new RedirectUrls
                {
                    cancel_url = $"{baseUrl}/paypal/cancel",
                    return_url = $"{baseUrl}/paypal/execute"
                }
            };
          



            return payment.Create(apiContext); ;
        }

        public async Task<string> CreatePayout(Guid eventId)
        {
            var eventEtity = await _unitOfWork.EventRepository.GetById(eventId);
            var payoutRequest = new PayoutRequest
            {
                items= new List<PayoutItem>
                {
                    new PayoutItem
                    {
                        amount = new Currency
                        {
                            currency = "USD",
                            value = eventEtity.Fare.ToString(),
                        }
                    }
                },

                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = GenerateSenderBatchId(),
                    email_subject = "You have a payout!",
                    
                }
            };

            
            var responseObject = JsonSerializer.Deserialize<PayoutResponse>(payoutRequest.ToString());

            return responseObject?.batch_header?.payout_batch_id;
        }

        private APIContext GetApiContext()
        {
            var config = new Dictionary<string, string>
        {
            { "mode", "sandbox" }, // Change to "live" in production
            { "clientId",  _configuration["PayPal:ClientId"]},
            { "clientSecret", _configuration["PayPal:Secret"] }
        };

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken);
        }

        public static string GenerateSenderBatchId()
        {
            Random random = new Random();
            /**
             * Generates a unique sender_batch_id based on the current date and a sequential number.
             * 
             * Returns:
             *     A unique sender_batch_id string.
             */
            DateTime today = DateTime.Today;
            string dateStr = today.ToString("yyyyMMdd");

            // Retrieve the last used number from a database or a file
            int randomNumber = random.Next(100000, 999999);

            string senderBatchId = $"{dateStr}_{randomNumber.ToString("D5")}";

            return senderBatchId;
        }

    }
}
