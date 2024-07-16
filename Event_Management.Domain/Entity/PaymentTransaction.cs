using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class PaymentTransaction
    {
        public Guid Id { get; set; }
        public string? PayId { get; set; }
        public string? EmailPaypal { get; set; }
        public string? TranMessage { get; set; }
        public string? TranStatus { get; set; }
        public decimal? TranAmount { get; set; }
        public DateTime? TranDate { get; set; }
        public Guid? RemitterId { get; set; }
        public Guid? EventId { get; set; }

        public virtual Event? Event { get; set; }
        public virtual User? Remitter { get; set; }
    }
}
