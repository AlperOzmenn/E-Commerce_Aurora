using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        void Add(Comment comment);
        Task SaveAsync(); // async tercih et
    }
}