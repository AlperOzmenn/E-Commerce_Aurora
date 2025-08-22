using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Navigation properties

        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}