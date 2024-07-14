using Event_Management.Domain.Models.Common;

using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Repository.Common
{
    public class SQLExtendRepository<T> : SQLRepository<T> where T : class
    {
        private readonly EventManagementContext _context;

        public SQLExtendRepository(EventManagementContext context): base(context)
        {
            _context = context;
        }

       

        public async Task<PagedList<T>> GetAll(int page, int eachPage, string sortBy, bool isAscending = false)
        {
            var entities = await _context.Set<T>().PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

            return new PagedList<T>(entities, entities.Count, page, eachPage);

        }

        public async Task<PagedList<T>> GetAll(Expression<Func<T, bool>> predicate, int page, int eachPage, string sortBy, bool isAscending = true)
        {
            var entities = await _context.Set<T>()
                .Where(predicate)
                .PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

            return new PagedList<T>(entities, entities.Count, page, eachPage);

        }

       
    }
}
