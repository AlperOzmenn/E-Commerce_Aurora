using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Address : BaseEntity
    {
        // Private alanlar
        private string _addressTitle;
        private string _addressText;
        private string? _region;

        // Public özellikler (property'ler) - encapsulated
        public string AddressTitle
        {
            get { return _addressTitle; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _addressTitle = value;
                else
                    throw new ArgumentException("Adres başlığı boş olamaz.");
            }
        }

        public string AddressText
        {
            get { return _addressText; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _addressText = value;
                else
                    throw new ArgumentException("Adres metni boş olamaz.");
            }
        }

        public CityEnum City { get; set; }

        public string? Region
        {
            get => _region;
            set
            {
                if (value?.Length > 100)
                    throw new ArgumentException("Bölge adı çok uzun.");
                _region = value;
            }
        }

        //public string AddressTitle { get; set; }
        //public string AddressText { get; set; }
        //public string City { get; set; }
        //public string? Region { get; set; }
        //public string Country { get; set; }

        // Navigation properties
        public virtual ICollection<AddressAppUser> AddressAppUsers { get; set; } = new List<AddressAppUser>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}