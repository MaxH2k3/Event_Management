using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Domain
{
    [Keyless]
    [Table("EventPayment")]
    public partial class EventPayment
    {
        [Column("PaymentID")]
        public Guid? PaymentId { get; set; }
        [Column("EventID")]
        public Guid? EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event? Event { get; set; }
        [ForeignKey(nameof(PaymentId))]
        public virtual Payment? Payment { get; set; }
    }
}
