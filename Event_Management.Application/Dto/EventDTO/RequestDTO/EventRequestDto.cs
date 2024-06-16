using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Event_Management.Application.Validators;

namespace Event_Management.Application
{
    public class EventRequestDto
    {

        [Required(ErrorMessage = "EventName is required!")]
        [MaxLength(250, ErrorMessage = "Event name is too long!")]
        [MinLength(3, ErrorMessage = "Event name is too short!")]
        public string EventName { get; set; } = null!;

        [Required(ErrorMessage = "Event Description is required!")]
        [MaxLength(5000, ErrorMessage = "Event Description is too long!")]
        [MinLength(3, ErrorMessage = "Event Description is too short!")]
        public string? Description { get; set; } = null!;

        //[Required]
        //public string? Status { get; set; }
        //[Required]
        //public Guid? CreatedBy { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1);
        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(2);

        public string? Image { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        public EventLocation? Location { get; set; }

        [Required(ErrorMessage = "Capacity is required!")]
        public int? Capacity { get; set; } = 30;

        public bool? Approval { get; set; } = false;

        [Range(0, 5000000, ErrorMessage = "Maximum ticket price is 5 000 000")]
        public double Ticket { get; set; } = 0;
    }
}
