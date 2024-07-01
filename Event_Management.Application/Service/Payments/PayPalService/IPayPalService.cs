namespace Event_Management.Application.Service.Payments.PayPalService
{
    public interface IPayPalService
    {
        Task<PayPal.Api.Payment> CreatePayment(Guid eventId, string description);

        Task<string> CreatePayout(Guid eventId);
    }
}
