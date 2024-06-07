using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("PaymentMethod")]
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("PaymentMethodID")]
        public int PaymentMethodId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? PaymentMethodName { get; set; }
        public bool? PaymentMethodStatus { get; set; }

        [InverseProperty(nameof(Payment.PaymentMethod))]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
