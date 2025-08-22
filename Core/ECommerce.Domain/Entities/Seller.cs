using System.Text.RegularExpressions;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities
{
    public class Seller : BaseEntity
    {
        private string _companyName;
        private string _contactName;
        private string _contactTitle;

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Firma adı boş bırakılamaz.");

                if (value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("Firma adı 3 ile 100 karakter arasında olmalıdır.");

                if (!Regex.IsMatch(value, @"^[a-zA-Z0-9ğüşöçİĞÜŞÖÇ\s\.\-&]+$"))
                    throw new ArgumentException("Firma adı yalnızca harf, rakam, boşluk ve bazı özel karakterler (.,-,&) içerebilir.");

                _companyName = value;
            }
        }

        public string ContactName
        {
            get => _contactName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("İlgili kişi adı boş bırakılamaz.");

                if (value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("İlgili kişi adı 3 ile 100 karakter arasında olmalıdır.");

                if (!Regex.IsMatch(value, @"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$"))
                    throw new ArgumentException("İlgili kişi adı yalnızca harf ve boşluk içermelidir.");

                _contactName = value;
            }
        }
        public string ContactTitle
        {
            get => _contactTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Unvan boş bırakılamaz.");

                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("Unvan 2 ile 100 karakter arasında olmalıdır.");

                if (!Regex.IsMatch(value, @"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$"))
                    throw new ArgumentException("Unvan yalnızca harf ve boşluk içermelidir.");

                _contactTitle = value;
            }
        }

        public bool Approved { get; set; } = false;
        public bool AuroraCheck { get; set; } = false;
        public Guid AppUserId { get; set; }

        // Nav Prop
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual AppUser AppUser { get; set; }
    }
}