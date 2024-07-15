using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models
{
    public class TransactionRequestDto
    {
        public string? PayId { get; set; }
        public string? PayerId { get; set; }
        public string? TranMessage { get; set; }
        public decimal? TranAmount { get; set; }
        public Guid? EventId { get; set; }

    }
}
