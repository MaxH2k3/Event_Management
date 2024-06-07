using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PackageDto
{
    public class PackageDto
    {
        [Required]
        public Guid PackageId { get; set; }

        [Required]
        public Guid? EventId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0.")]
        public double? Budget { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Event Description is required!")]
        [MaxLength(50, ErrorMessage = "Event Description is too long!")]
        [MinLength(3, ErrorMessage = "Event Description is too short!")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TotalTransaction must be greater or equal than 1.")]
        public int? TotalTransaction { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? PackageType { get; set; }
    }
}
