﻿using Azure;
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
        public async Task<PayPal.Api.Payment> CreatePayment(Guid eventId, Guid userId, string description)
        {
            
            var apiContext = GetApiContext();
            string baseUrl = "https://localhost:7153";
            var eventEntity = await _unitOfWork.EventRepository.GetById(eventId);
            var sponsor = await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(eventId, userId);

			string totalAmount = sponsor?.Amount?.ToString() ?? eventEntity.Fare.ToString();

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
                        total = totalAmount
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

        public async Task<PayoutBatchHeader> CreatePayout(Guid eventId, string emailReceiver)
        {
            var apiContext = GetApiContext();
            var eventEtity = await _unitOfWork.EventRepository.GetById(eventId);
            var payoutBatchHeader = new PayoutBatchHeader();
            var payoutRequest = new Payout
            {
                items = new List<PayoutItem>
                {

                    new PayoutItem
                    {
                        receiver = emailReceiver,
                        recipient_type = PayoutRecipientType.EMAIL,
                        amount = new Currency
                        {
                            currency = "USD",
                            value = "5.00",
                        },
                        note = "Thanks for your participation!",
                        sender_item_id = $"{DateTime.UtcNow.Ticks}-{new Random().Next(1000, 9999)}",
                        
                    }
                },

                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = GenerateSenderBatchId(),
                    email_subject = "You have a payout!",

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