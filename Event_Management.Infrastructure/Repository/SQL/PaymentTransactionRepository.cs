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
using Event_Management.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Event_Management.Infrastructure.Extensions;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class PaymentTransactionRepository : SQLRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly EventManagementContext _context;

        public PaymentTransactionRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedList<PaymentTransaction>> GetMyEventTransaction(Guid eventId, int page, int eachPage)
        {
            var list = _context.PaymentTransactions.Where(p => p.EventId.Equals(eventId)).OrderByDescending(p => p.TranDate);

            return await list.ToPagedListAsync(page, eachPage);
        }

        public async Task<PagedList<PaymentTransaction>> GetMyTransaction(Guid userId, int page, int eachPage)
        {
            var list = _context.PaymentTransactions.Where(p => p.Remitter.Equals(userId)).OrderByDescending(p => p.TranDate);

            return await list.ToPagedListAsync(page, eachPage);
        }
    }
}
