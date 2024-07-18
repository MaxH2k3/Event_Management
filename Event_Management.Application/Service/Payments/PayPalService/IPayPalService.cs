using Event_Management.Application.Dto.PaymentDTO.PayPalPayment;
using Event_Management.Domain.Models.Payment;
using PayPal.Api;

namespace Event_Management.Application.Service.Payments.PayPalService
{
    public interface IPayPalService
    {
        Task<PayPal.Api.Payment> CreatePayment(CreatePaymentDto createPaymentDto, Guid userId);

        Task<PayoutBatchHeader> CreatePayoutUnstable(PayoutDto payoutDto);

        Task<PayoutBatchHeader> CreatePayoutById(PayoutSponsorDto payoutSponsorDto);
    }
}
