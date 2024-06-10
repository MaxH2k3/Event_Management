using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? EventId { get; set; }
        public Guid? PaymentId { get; set; }
        public double? Money { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }

        public virtual Event? Event { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
    }
}
