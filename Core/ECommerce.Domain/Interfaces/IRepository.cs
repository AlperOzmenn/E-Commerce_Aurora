using ECommerce.Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, bool ignoreFilters = false);
        Task<IEnumerable<T>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false);
        Task<IEnumerable<T>> FindConditionAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);
        IQueryable<T> GetQuery();
        Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);
        Task<int> CountAsync(Expression<Func<T, bool>> condition = null);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        void SoftDelete(T item);
    }
}