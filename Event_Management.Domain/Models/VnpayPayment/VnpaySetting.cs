using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Event_Management.Domain.Model.VnpayPayment
{
    public class VnpaySetting
    {
        public string ConfigName { get; set; } = "Vnpay";
        public string Version { get; set; } = "2.1.0";
        public string TmnCode { get; set; } = "XDI3OIRC";
        public string HashSecret { get; set; } = "EM98KV6UQQ5O7IEDJEQNDLNORN6S976J";
        public string ReturnUrl { get; set; } = "http://localhost:7153/api/payment/vnpay-return";
        public string PaymentUrl { get; set; } = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";


        //public string Version { get; set; }
        //public string TmnCode { get; set; }
        //public string HashSecret { get; set; }
        //public string ReturnUrl { get; set; }
        //public string PaymentUrl { get; set; }
    }
}
