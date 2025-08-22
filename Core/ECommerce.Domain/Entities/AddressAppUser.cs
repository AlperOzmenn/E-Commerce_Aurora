using ECommerce.Domain.Entities.Commons;

namespace ECommerce.Domain.Entities
{
    public class AddressAppUser : BaseEntity
    {
        public bool IsDefault { get; set; } = false;
        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }

    }
}