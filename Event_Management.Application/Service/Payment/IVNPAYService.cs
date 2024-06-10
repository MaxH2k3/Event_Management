using Event_Management.Application.Dto.PaymentDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Payment
{
    public interface IVNPAYService
    {
        string CreatePaymentUrl(VNPAYPaymentRequest request);
        bool VerifyPayment(HttpRequest request);
    }
}
