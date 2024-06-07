using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("SponsorMethod")]
    public partial class SponsorMethod
    {
        [Key]
        public int SponsorMethodId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? SponsorMethodName { get; set; }
        [Column("PackageID")]
        public Guid? PackageId { get; set; }

        [ForeignKey(nameof(PackageId))]
        [InverseProperty("SponsorMethods")]
        public virtual Package? Package { get; set; }
    }
}
