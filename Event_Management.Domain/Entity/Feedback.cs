using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Feedback")]
    public partial class Feedback
    {
        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Key]
        [Column("EventID")]
        public Guid EventId { get; set; }
        [StringLength(5000)]
        [Unicode(false)]
        public string? Content { get; set; }
        public int? Rating { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(EventId))]
        [InverseProperty("Feedbacks")]
        public virtual Event Event { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Feedbacks")]
        public virtual User User { get; set; } = null!;
    }
}
