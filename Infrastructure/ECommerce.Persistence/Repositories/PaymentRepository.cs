using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Interfaces;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class PaymentRepository : EfRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
