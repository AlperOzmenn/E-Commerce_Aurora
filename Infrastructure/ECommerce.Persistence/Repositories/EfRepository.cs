using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.AnyAsync(condition);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> condition = null)
        {
            if (condition == null)
                return _dbSet.CountAsync();

            return _dbSet.CountAsync(condition);
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public async Task<IEnumerable<T>> FindConditionAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.Where(condition).ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreFilters)
                query = query.IgnoreQueryFilters();

            if (!isTrack)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id, bool ignoreFilters = false)
        {
            if (ignoreFilters)
                return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);

            return await _dbSet.FindAsync(id);
        }

        public async Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _dbSet;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync();
            else
                return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _dbSet;

            if (join != null)
                query = join(query);

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }

        public void SoftDelete(T item)
        {
            item.SoftDelete();
            _dbSet.Update(item);
        }

        public void Update(T item)
        {
            _dbSet.Update(item);
        }
    }
}