using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service
{
    public interface IPaymentService
    {
        //Task<bool> AddPayment(PaymentDto paymentDto);
        Task<Payment?> GetPaymentById(Guid id);
    }
}
