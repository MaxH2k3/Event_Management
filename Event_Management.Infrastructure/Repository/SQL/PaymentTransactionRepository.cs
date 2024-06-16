using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Event_Management.Domain;
using Event_Management.Domain.Entity;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class PaymentTransactionRepository : SQLRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly EventManagementContext _context;

        public PaymentTransactionRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}
