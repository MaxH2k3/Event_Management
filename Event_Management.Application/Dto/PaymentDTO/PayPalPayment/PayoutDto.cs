using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PaymentDTO.PayPalPayment
{
    public class PayoutDto
    {
        public Guid EventId {  get; set; }
        public string EmailReceiver {get; set; }
        public decimal Amount { get; set; }
        public string EmailSubject { get; set; }
        
    }
}
