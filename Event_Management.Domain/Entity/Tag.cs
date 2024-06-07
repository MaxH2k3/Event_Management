using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            Events = new HashSet<Event>();
        }

        [Key]
        [Column("TagID")]
        public int TagId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? TagName { get; set; }

        [ForeignKey("TagId")]
        [InverseProperty(nameof(Event.Tags))]
        public virtual ICollection<Event> Events { get; set; }
    }
}
