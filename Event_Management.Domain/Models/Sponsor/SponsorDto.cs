using Event_Management.Domain.Enum.Sponsor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Sponsor
{ 
	public class SponsorDto
	{
		public Guid EventId { get; set; }
		//public int? SponsorMethodId { get; set; }
        public string? SponsorType { get; set; }
        public decimal? Amount { get; set; }
        [StringLength(200, ErrorMessage = "Message cannot exceed 200 characters.")]
        public string? Message { get; set; }
		
		
	}
}
