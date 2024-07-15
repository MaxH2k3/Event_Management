using Event_Management.Domain.Entity;
using Event_Management.Domain.Models;

namespace Event_Management.Application.Service
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransaction> AddTransaction(TransactionRequestDto transactionRequestDto, Guid userId);
    }
}
