using Event_Management.Domain.Entity;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Application.Service
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransaction> AddTransaction(TransactionRequestDto transactionRequestDto);
        Task<PagedList<PaymentTransaction>> GetAllTransaction(int page, int eachPage);
    }
}
