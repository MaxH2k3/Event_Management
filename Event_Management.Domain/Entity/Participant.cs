using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Participant")]
    public partial class Participant
    {
        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Key]
        [Column("EventID")]
        public Guid EventId { get; set; }
        [Column("RoleEventID")]
        public int? RoleEventId { get; set; }
        public DateTime? CheckedIn { get; set; }
        public bool? IsCheckedMail { get; set; }
        public DateTime CreatedAt { get; set; }

		[ForeignKey(nameof(EventId))]
        [InverseProperty("Participants")]
        public virtual Event Event { get; set; } = null!;
        [ForeignKey(nameof(RoleEventId))]
        [InverseProperty("Participants")]
        public virtual RoleEvent? RoleEvent { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Participants")]
        public virtual User User { get; set; } = null!;
    }
}
