using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Payment.StripePayment
{
    public class CheckoutOrderResponse
    {
        public string? SessionId { get; set; }
        public string? PubKey { get; set; }
    }
}
