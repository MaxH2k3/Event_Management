﻿using Event_Management.Application.Message;
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

        public async Task<PaymentTransaction> AddTransaction(TransactionRequestDto transactionRequestDto)
        {
           
            var newTransaction = new PaymentTransaction();
            newTransaction.RemitterId = transactionRequestDto.UserId;
            newTransaction.TranMessage = transactionRequestDto.TransMessage;
            newTransaction.PayId = transactionRequestDto.PayId;
            newTransaction.EmailPaypal = transactionRequestDto.EmailPaypal;
            newTransaction.TranAmount = transactionRequestDto.TransAmount;
            newTransaction.TranDate = DateTimeHelper.GetDateTimeNow();
            newTransaction.TranStatus = MessagePayment.TranStatus;
            await _unitOfWork.PaymentTransactionRepository.Add(newTransaction);
            await _unitOfWork.SaveChangesAsync();
            return newTransaction;

        }
    }
}
