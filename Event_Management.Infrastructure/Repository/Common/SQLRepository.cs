using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Event_Management.Infrastructure.Repository.Common
{
	public class SQLRepository<T> : IRepository<T> where T : class
    {
        private readonly EventManagementContext _context;

        public SQLRepository(EventManagementContext context)
        {
            _context = context;
        }

       

        //Base Repository
        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(dynamic id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return;
            }
            _context.Set<T>().Remove(entity);
        }

        public async Task Delete(params dynamic[] id)
        {
			var entity = await _context.Set<T>().FindAsync(id);
			if (entity == null)
			{
				return;
			}

			_context.Set<T>().Remove(entity);
		}

		public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PagedList<T>> GetAll(int page, int eachPage)
        {
            var list = await _context.Set<T>().ToListAsync();
            var totalItems = list.Count;
            var items = list.Skip((page - 1) * eachPage).Take(eachPage);

            return new PagedList<T>(items, totalItems, page, eachPage);
        }

        public async Task<PagedList<T>> GetAll(Expression<Func<T, bool>> predicate, int page, int eachPage)
        {
            var list = await _context.Set<T>().Where(predicate).ToListAsync();
            var totalItems = list.Count;
            var items = list.Skip((page - 1) * eachPage).Take(eachPage);

            return new PagedList<T>(items, totalItems, page, eachPage);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> GetById(dynamic id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry.State = EntityState.Modified;
            }

            return Task.CompletedTask;
        }
    }
}
