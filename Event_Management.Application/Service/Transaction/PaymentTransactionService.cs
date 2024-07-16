using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.UnitOfWork;

namespace Event_Management.Application.Service
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentTransaction> AddTransaction(TransactionRequestDto transactionRequestDto, Guid userId)
        {
           
            var newTransaction = new PaymentTransaction();
            newTransaction.RemitterId = userId;
            newTransaction.TranMessage = transactionRequestDto.TranMessage;
            newTransaction.PayId = transactionRequestDto.PayId;
            newTransaction.PayerId = transactionRequestDto.PayerId;
            newTransaction.TranAmount = transactionRequestDto.TranAmount;
            newTransaction.TranDate = DateTimeHelper.GetDateTimeNow();
            await _unitOfWork.PaymentTransactionRepository.Add(newTransaction);
            await _unitOfWork.SaveChangesAsync();
            return newTransaction;

        }
    }
}
