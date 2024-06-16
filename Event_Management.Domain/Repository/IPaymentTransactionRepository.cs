using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Repository
{
    public interface IPaymentTransactionRepository : IRepository<PaymentTransaction>
    {
    }
}
