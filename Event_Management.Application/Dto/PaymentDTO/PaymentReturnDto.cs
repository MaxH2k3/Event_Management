using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.PaymentDTO
{
    public class PaymentReturnDto
    {
        public Guid PaymentId { get; set; }
        /// <summary>
        /// 00: Success
        /// 99: Unknown
        /// 10: Error
        /// </summary>
        public string? PaymentStatus { get; set; }
        public string? PaymentMessage { get; set; }
        /// <summary>
        /// Format: yyyyMMddHHmmss
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        public string? PaymentRefId { get; set; }
        public decimal? RequiredAmount { get; set; }
        //public string? Signature { get; set; }
    }
}
