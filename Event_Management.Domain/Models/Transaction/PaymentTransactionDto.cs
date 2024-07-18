using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Transaction
{
    public class PaymentTransactionDto
    {
        public Guid Id { get; set; }
        public string? PayId { get; set; }
        public string? EmailPaypal { get; set; }
        public string? TranMessage { get; set; }
        public string? TranStatus { get; set; }
        public decimal? TranAmount { get; set; }
        public DateTime? TranDate { get; set; }
        public Guid? RemitterId { get; set; }
        public Guid? EventId { get; set; }
        public string? EmailAccount { get; set; }
        public string? EventName { get; set; }

    }
}
