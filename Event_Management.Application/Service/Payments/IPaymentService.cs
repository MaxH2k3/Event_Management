using Event_Management.Application.Dto.PaymentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service
{
    public interface IPaymentService
    {
        Task<bool> AddPayment(PaymentDto paymentDto);
        Task<bool> GetPaymentById(string id);
    }
}
