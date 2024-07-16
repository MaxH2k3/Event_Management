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
        public string? EmailPaypal { get; set; }
        public Guid UserId {  get; set; }
        public string? TransMessage { get; set; }
        public decimal? TransAmount { get; set; }
        public Guid EventId { get; set; }

    }
}
