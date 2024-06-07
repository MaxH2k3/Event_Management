using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("RoleEvent")]
    public partial class RoleEvent
    {
        public RoleEvent()
        {
            Participants = new HashSet<Participant>();
        }

        [Key]
        [Column("RoleEventID")]
        public int RoleEventId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? RoleEventName { get; set; }

        [InverseProperty(nameof(Participant.RoleEvent))]
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
