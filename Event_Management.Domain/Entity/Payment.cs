using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid PaymentId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentOwner { get; set; }
        public Guid? UserId { get; set; }
        public string? SerialNumber { get; set; }
        public bool? PaymentStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
