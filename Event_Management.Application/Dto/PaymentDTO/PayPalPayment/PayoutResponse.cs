using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PaymentDTO.PayPalPayment
{
    public class PayoutResponse
    {
        public PayoutBatchHeader batch_header { get; set; }
    }
}
