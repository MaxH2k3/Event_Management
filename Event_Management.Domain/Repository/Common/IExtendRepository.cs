using Event_Management.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Repository.Common
{
    public interface IExtendRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities from database with condition, sort and pagination
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="eachPage"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        Task<PagedList<T>> GetAll(Expression<Func<T, bool>> predicate, int page, int eachPage, string sortBy, bool isAscending = false);

        /// <summary>
        /// Get all entities from database with sort and pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="eachPage"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        Task<PagedList<T>> GetAll(int page, int eachPage, string sortBy, bool isAscending = false);
    }
}
