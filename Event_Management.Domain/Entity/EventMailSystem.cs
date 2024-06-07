using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Domain
{
    [Keyless]
    [Table("EventMailSystem")]
    public partial class EventMailSystem
    {
        [Column("EventID")]
        public Guid? EventId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TimeExecute { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? MethodKey { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string? Description { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Title { get; set; }
        [Unicode(false)]
        public string? Body { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Type { get; set; }
        public Guid? CreatedBy { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event? Event { get; set; }
    }
}
