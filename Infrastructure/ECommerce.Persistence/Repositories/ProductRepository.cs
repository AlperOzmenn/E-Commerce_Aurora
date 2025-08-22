using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.Interfaces;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}