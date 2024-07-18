using AutoMapper;
using Event_Management.Application.Message;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Models.Transaction;
using Event_Management.Domain.UnitOfWork;

namespace Event_Management.Application.Service
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TransactionRequestDto> AddTransaction(TransactionRequestDto transactionRequestDto)
        {
           
            var newTransaction = new PaymentTransaction();
            newTransaction.Id = Guid.NewGuid();
            newTransaction.RemitterId = transactionRequestDto.UserId;
            newTransaction.TranMessage = transactionRequestDto.TransMessage;
            newTransaction.PayId = transactionRequestDto.PayId;
            newTransaction.EventId = transactionRequestDto.EventId;
            newTransaction.EmailPaypal = transactionRequestDto.EmailPaypal;
            newTransaction.TranAmount = transactionRequestDto.TransAmount;
            newTransaction.TranDate = DateTimeHelper.GetDateTimeNow();
            newTransaction.TranStatus = MessagePayment.TranStatus;
            await _unitOfWork.PaymentTransactionRepository.Add(newTransaction);
            await _unitOfWork.SaveChangesAsync();
            return transactionRequestDto;

        }

        public async Task<PagedList<PaymentTransactionDto>> GetAllTransaction(int page, int eachPage)
        {
            var transactionList = await _unitOfWork.PaymentTransactionRepository.GetAll(page, eachPage);   
            var transactionDto = _mapper.Map<PagedList<PaymentTransactionDto>>(transactionList);
            return transactionDto;
        }

        public async Task<PagedList<PaymentTransactionDto>> GetMyEventTransaction(Guid eventId, int page, int eachPage)
        {
            var transactionList =  await _unitOfWork.PaymentTransactionRepository.GetMyEventTransaction(eventId, page, eachPage);
            var transactionDto = _mapper.Map<PagedList<PaymentTransactionDto>>(transactionList);
            return transactionDto;
        }

        public async Task<PagedList<PaymentTransactionDto>> GetMyTransaction(Guid userId, int page, int eachPage)
        {
            var transactionList = await _unitOfWork.PaymentTransactionRepository.GetMyTransaction(userId, page, eachPage);
            var transactionDto = _mapper.Map<PagedList<PaymentTransactionDto>>(transactionList);
            return transactionDto;
        }

        public async Task<PaymentTransaction?> GetTransactionById(Guid transactionId)
        {
            return await _unitOfWork.PaymentTransactionRepository.GetById(transactionId);
        }
    }
}
