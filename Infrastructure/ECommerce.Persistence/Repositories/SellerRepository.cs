using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class SellerRepository : EfRepository<Seller>, ISellerRepository
    {
        public SellerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
