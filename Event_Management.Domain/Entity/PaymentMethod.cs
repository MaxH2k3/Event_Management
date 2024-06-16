using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
        public bool? PaymentMethodStatus { get; set; }
    }
}
