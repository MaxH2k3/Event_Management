using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Keyless]
    public partial class Perk
    {
        [Column("PackageID")]
        public Guid? PackageId { get; set; }
        [Column("PolicyID")]
        public int? PolicyId { get; set; }

        [ForeignKey(nameof(PackageId))]
        public virtual Package? Package { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public virtual Policy? Policy { get; set; }
    }
}
