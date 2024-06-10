using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }

        public int PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
        public bool? PaymentMethodStatus { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
