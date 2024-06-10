using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class EventPayment
    {
        public Guid? PaymentId { get; set; }
        public Guid? EventId { get; set; }

        public virtual Event? Event { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
