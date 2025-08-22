using ECommerce.Domain.Entities.Commons;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        // Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}