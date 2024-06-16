using AutoMapper;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> AddPayment(PaymentDto paymentDto)
        {
            var paymentEntity = _mapper.Map<Payment>(paymentDto);
            await _unitOfWork.PaymentRepository.Add(paymentEntity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> GetPaymentById(string id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetById(id);
            if (payment != null)
            {
                return true;
            }
            return false;
        }
    }
}
