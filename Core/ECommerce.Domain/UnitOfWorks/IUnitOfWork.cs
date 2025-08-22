using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Domain.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUserRepository { get; }
        IRepository<T> GetRepository<T>() where T : class, IBaseEntity;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}