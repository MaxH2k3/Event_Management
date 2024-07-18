using Event_Management.Domain.Entity;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Application.Service
{
    public interface IPaymentTransactionService
    {
        Task<TransactionRequestDto> AddTransaction(TransactionRequestDto transactionRequestDto);
        Task<PagedList<PaymentTransaction>> GetAllTransaction(int page, int eachPage);
        Task<PagedList<PaymentTransaction>> GetMyTransaction(Guid userId, int page, int eachPage);
        Task<PagedList<PaymentTransaction>> GetMyEventTransaction(Guid eventId, int page, int eachPage);
    }
}
