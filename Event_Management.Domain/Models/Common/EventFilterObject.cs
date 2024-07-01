using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Common
{
    public class EventFilterObject
    {
        public string? UserCoord { get; set; } = null;
        public string? EventName { get; set; } = null;
        public string? Status { get; set; } = null ;
        public long? StartDateFrom { get; set; } = null;
        public long? StartDateTo { get; set; } = null;
        public long? EndDateFrom { get; set; } = null;
        public long? EndDateTo { get; set; } = null;
        public string? Location { get; set; } = null;
        public bool? Approval { get; set; } = null;
        public decimal? TicketFrom { get; set; } = null;
        public decimal? TicketTo { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsAscending { get;} = false;
    }
}
