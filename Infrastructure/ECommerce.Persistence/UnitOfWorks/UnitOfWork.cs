using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repositories;

namespace ECommerce.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork

    {

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context, IAppUserRepository appUserRepository)
        {
            _context = context;
            AppUserRepository = appUserRepository;
        }

        public IAppUserRepository AppUserRepository { get; }

        public IRepository<T> GetRepository<T>() where T : class, IBaseEntity
        {
            return new EfRepository<T>(_context);
        }

        public int SaveChanges()
        {
            var result = _context.SaveChanges();

            if (result > 0)
                return result;

            else
                throw new Exception("Değişiklik işlenemedi!");
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return result;
            else
                throw new Exception("Değişiklik işlenemedi!");
        }
    }
}