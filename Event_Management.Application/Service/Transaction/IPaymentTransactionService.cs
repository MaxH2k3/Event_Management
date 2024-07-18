using Event_Management.Domain.Entity;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Transaction;

namespace Event_Management.Application.Service
{
    public interface IPaymentTransactionService
    {
        Task<TransactionRequestDto> AddTransaction(TransactionRequestDto transactionRequestDto);
        Task<PagedList<PaymentTransactionDto>> GetAllTransaction(int page, int eachPage);
        Task<PagedList<PaymentTransactionDto>> GetMyTransaction(Guid userId, int page, int eachPage);
        Task<PagedList<PaymentTransactionDto>> GetMyEventTransaction(Guid eventId, int page, int eachPage);
        Task<PaymentTransaction?> GetTransactionById(Guid transactionId);
    }
}
