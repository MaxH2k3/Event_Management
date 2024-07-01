using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PaymentDTO
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string? PaymentContent { get; set; }
        public string? PaymentCurrency { get; set; }
        public string? PaymentRefId { get; set; }
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; }
        public string? PaymentDestinationId { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? PaymentLastMessage { get; set; }
        public Guid? CreatedBy { get; set; }
       
        public string? PaymentPurpose { get; set; }
    }
}
