using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Kategori adı boş bırakılamaz.");

                if (value.Length > 50)
                    throw new ArgumentException("Kategori adı en fazla 50 karakter olabilir.");


                _name = value;
            }
        }

        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}