using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("TransactionID")]
        public int TransactionId { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column("EventID")]
        public Guid? EventId { get; set; }
        [Column("PaymentID")]
        public Guid? PaymentId { get; set; }
        public double? Money { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Type { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Status { get; set; }

        [ForeignKey(nameof(EventId))]
        [InverseProperty("Transactions")]
        public virtual Event? Event { get; set; }
        [ForeignKey(nameof(PaymentId))]
        [InverseProperty("Transactions")]
        public virtual Payment? Payment { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Transactions")]
        public virtual User? User { get; set; }
    }
}
