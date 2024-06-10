using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Event_Management.Application.Dto.PaymentDTO
{
    public class VNPAYPaymentRequest : IRequest<PaymentLinkDto>
    {
        public string PaymentCurrency { get; set; } = string.Empty;
        public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequireAmount { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PaymentLanguage { get; set; } = string.Empty;
        public string? MerchantId { get; set; } = string.Empty;
        public string? PaymentDestinationId { get; set; } = string.Empty;
       

    }

    public class CreatePaymentHandler : IRequestHandler<VNPAYPaymentRequest, PaymentLinkDto>
    {
        public Task<PaymentLinkDto> Handle(VNPAYPaymentRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
