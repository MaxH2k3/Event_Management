using PayPal.Api;

namespace Event_Management.Application.Service.Payments.PayPalService
{
    public interface IPayPalService
    {
        Task<PayPal.Api.Payment> CreatePayment(Guid eventId, Guid userId, string description);

        Task<PayoutBatchHeader> CreatePayout(Guid eventId, string emailReceiver);
    }
}
