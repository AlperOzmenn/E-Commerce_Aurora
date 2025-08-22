using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        private string? _description;
        public AppRole() { }

        public AppRole(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpper();
        }
        public string? Description // Role ait kişinin yetkileri ya da yapabileceği işlemlerin açıklaması yazılır.
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentException("Açıklama en fazla 200 karakter olabilir.");

                _description = value;
            }
        }

        // Navigation properties
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}