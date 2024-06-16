
using MediatR;


namespace Event_Management.Application.Dto.PaymentDTO
{
    public class PaymentDto : IRequest<PaymentLinkDto>
    {
       
        public string PaymentContent { get; set; }
        public string PaymentCurrency { get; set; } 
        public string PaymentRefId { get; set; }
        public decimal? RequiredAmount { get; set; }
        //public DateTime? PaymentDate { get; set; } = DateTime.Now;
        //public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PaymentLanguage { get; set; } 
        public Guid CreatedBy { get; set; }
        public string? PaymentDestinationId { get; set; } 
        //public string? Signature { get; set; } = string.Empty;
        //public decimal? PaidAmount { get; set; }
        public string? PaymentStatus { get; set; } = string.Empty;

    }
}
