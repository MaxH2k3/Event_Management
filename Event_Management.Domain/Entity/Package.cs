
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Package")]
    public partial class Package
    {
        public Package()
        {
            SponsorMethods = new HashSet<SponsorMethod>();
        }

        [Key]
        [Column("PackageID")]
        public Guid PackageId { get; set; }
        [Column("EventID")]
        public Guid? EventId { get; set; }
        public double? Budget { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Description { get; set; }
        public int? TotalTransaction { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedAt { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? PackageType { get; set; }

        [ForeignKey(nameof(EventId))]
        [InverseProperty("Packages")]
        public virtual Event? Event { get; set; }
        [InverseProperty(nameof(SponsorMethod.Package))]
        public virtual ICollection<SponsorMethod> SponsorMethods { get; set; }
    }
}
