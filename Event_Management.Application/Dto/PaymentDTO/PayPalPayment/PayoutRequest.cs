using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PaymentDTO
{
    public class PayoutRequest
    {
        public PayoutSenderBatchHeader sender_batch_header { get; set; }
        public List<PayoutItem> items { get; set; }

        
    }
}
