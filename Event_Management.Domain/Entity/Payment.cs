using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Payment")]
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("PaymentID")]
        public Guid PaymentId { get; set; }
        [Column("PaymentMethodID")]
        public int? PaymentMethodId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? PaymentOwner { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? SerialNumber { get; set; }
        public bool? PaymentStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        [InverseProperty("Payments")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Payments")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(Transaction.Payment))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
