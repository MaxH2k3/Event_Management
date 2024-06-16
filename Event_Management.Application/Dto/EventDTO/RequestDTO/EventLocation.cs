using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application
{
    public class EventLocation
    {
        [Required(ErrorMessage = "Location is required!")]
        [MaxLength(500, ErrorMessage = "Location is too long!")]
        [MinLength(5, ErrorMessage = "Location is too short!")]
        public string Location { get; set; } = "FPTUHCM";


        [Required(ErrorMessage = "LocationId is required!")]
        public string? LocationId { get; set; } = null;


        [Required(ErrorMessage = "LocationAddress is required!")]
        public string? LocationAddress { get; set; } = null;

        [Required(ErrorMessage = "LocationUrl is required!")]
        public string? LocationUrl { get; set; } = null;

        [Required(ErrorMessage = "LocationCoord is required!")]
        public string? LocationCoord { get; set; } = null;
    }
}
