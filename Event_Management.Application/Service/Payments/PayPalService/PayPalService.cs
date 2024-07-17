using Azure;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Dto.PaymentDTO.PayPalPayment;
using Event_Management.Application.Helper;
using Event_Management.Application.Message;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using RestSharp;
using System.Text.Json;

namespace Event_Management.Application.Service.Payments.PayPalService
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
       

        public PayPalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }
        public async Task<PayPal.Api.Payment> CreatePayment(CreatePaymentDto createPaymentDto, Guid userId)
        {
            
            var apiContext = GetApiContext();
            string eventIdUrl = createPaymentDto.EventId.ToString();
            string baseUrl = createPaymentDto.BaseUrl;

            //string baseUrl = "https://event-manage-nine.vercel.app";
            var eventEntity = await _unitOfWork.EventRepository.GetById(createPaymentDto.EventId);
            


            decimal? totalAmount;
            if(createPaymentDto.Amount > 0)
            {
                var sponsor = await _unitOfWork.SponsorEventRepository.CheckSponsoredEvent(createPaymentDto.EventId, userId);
                if(sponsor == null)
                {
                    return null;
                }
                totalAmount = createPaymentDto.Amount;
            }
            else
            {
                totalAmount = eventEntity!.Fare;
            }
            string apiUrl = "https://api.currencyapi.com/v3/latest?apikey=cur_live_YmCF5RSIievrfTvYMaZV82SIUD4zwtmW5asnZNI6&base_currency=USD&currencies=VND";
            string translateAmount = await CurrencyHelper.GetExchangeRate(apiUrl, totalAmount);
            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
            {
                new Transaction
                {
                    description = createPaymentDto.Message,
                    amount = new Amount
                    {
                        currency = "USD",
                        total = translateAmount
                    }
                }
            },
                redirect_urls = new RedirectUrls
                {
                    
                    cancel_url = $"{baseUrl}/events/{eventIdUrl}",
                    return_url = $"{baseUrl}/events/{eventIdUrl}"
                }
            };
          



            return payment.Create(apiContext); 
        }

        public async Task<PayoutBatchHeader> CreatePayout(PayoutDto payoutDto)
        {
            var apiContext = GetApiContext();
            var eventEtity = await _unitOfWork.EventRepository.GetById(payoutDto.EventId);
            string apiUrl = "https://api.currencyapi.com/v3/latest?apikey=cur_live_YmCF5RSIievrfTvYMaZV82SIUD4zwtmW5asnZNI6&base_currency=USD&currencies=VND";
            string translateAmount = await CurrencyHelper.GetExchangeRate(apiUrl, payoutDto.Amount);
            var payoutBatchHeader = new PayoutBatchHeader();
            var payoutRequest = new Payout
            {
                items = new List<PayoutItem>
                {

                    new PayoutItem
                    {
                        receiver = payoutDto.EmailReceiver,
                        recipient_type = PayoutRecipientType.EMAIL,
                        amount = new Currency
                        {
                            currency = "USD",
                            value = translateAmount,
                        },
                        note = MessagePayment.MessageNotification,
                        sender_item_id = $"{DateTime.UtcNow.Ticks}-{new Random().Next(1000, 9999)}",
                        
                    }
                },

                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = SystemHelper.GenerateSenderBatchId(),
                    email_subject = payoutDto.EmailSubject,

                }
            };

            var createdPayout = payoutRequest.Create(apiContext, false); //createdPayout?.batch_header?.payout_batch_id
                                                                         //var responseObject = JsonSerializer.Deserialize<PayoutResponse>(payoutRequest.ToString());



            
            var payoutBatch = new PayoutBatchHeader
            {
                payout_batch_id = createdPayout.batch_header.payout_batch_id,
                batch_status = createdPayout.batch_header.batch_status,
                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = payoutRequest.sender_batch_header.sender_batch_id,
                    email_subject = payoutRequest.sender_batch_header.email_subject,
                    recipient_type = payoutRequest.items[0].recipient_type,
                },
                amount = createdPayout.batch_header.amount,

            };

            
            return payoutBatch;
        }

        public APIContext GetApiContext()
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

        

        

    }
}
