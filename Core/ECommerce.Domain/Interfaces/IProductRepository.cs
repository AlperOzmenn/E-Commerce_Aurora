using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}