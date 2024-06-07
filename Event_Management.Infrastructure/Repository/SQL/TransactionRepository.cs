using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class TransactionRepository : SQLRepository<Transaction>, ITransactionRepository
    {
        private readonly EventManagementContext _context;

        public TransactionRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
    }
}
