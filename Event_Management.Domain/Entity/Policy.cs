using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Domain
{
    [Table("Policy")]
    public partial class Policy
    {
        [Key]
        [Column("PolicyID")]
        public int PolicyId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string PolicyName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string? Description { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string? Status { get; set; }
    }
}
