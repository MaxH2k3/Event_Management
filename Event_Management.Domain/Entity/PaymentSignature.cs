using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class PaymentSignature
    {
        public Guid Id { get; set; }
        public string? SignValue { get; set; }
        public DateTime? SignDate { get; set; }
        public string? SignAlgo { get; set; }
        public string? SignOwn { get; set; }
        public Guid? PaymentId { get; set; }
        public bool? IsValid { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
