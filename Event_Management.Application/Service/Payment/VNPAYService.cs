using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Payment
{
    public class VNPAYService : IVNPAYService
    {
        private readonly IConfiguration _configuration;

        public VNPAYService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentUrl(VNPAYPaymentRequest request)
        {
            var vnpayUrl = _configuration["VNPAY:VNPAYUrl"];
            var merchantId = _configuration["VNPAY:MerchantId"];
            var secretKey = _configuration["VNPAY:SecretKey"];
            var returnUrl = _configuration["VNPAY:ReturnUrl"];

            var vnpay = new VNPAYLibrary();

            //vnpay.AddRequestData("vnp_Version", "2.0.0");
            //vnpay.AddRequestData("vnp_Command", "pay");
            //vnpay.AddRequestData("vnp_TmnCode", merchantId);
            //vnpay.AddRequestData("vnp_Amount", ((int)(request.Amount * 100)).ToString());
            //vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //vnpay.AddRequestData("vnp_CurrCode", "VND");
            //vnpay.AddRequestData("vnp_IpAddr", request.IpAddress);
            //vnpay.AddRequestData("vnp_Locale", "vn");
            //vnpay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
            //vnpay.AddRequestData("vnp_OrderType", "other");
            //vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            //vnpay.AddRequestData("vnp_TxnRef", request.OrderId);

            string paymentUrl = vnpay.CreateRequestUrl(vnpayUrl, secretKey);
            return paymentUrl;
        }

        public bool VerifyPayment(HttpRequest request)
        {
            var vnpay = new VNPAYLibrary();

            foreach (var (key, value) in request.Query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            var secretKey = _configuration["VNPAY:SecretKey"];
            return vnpay.ValidateSignature(secretKey);
        }
    }
}
