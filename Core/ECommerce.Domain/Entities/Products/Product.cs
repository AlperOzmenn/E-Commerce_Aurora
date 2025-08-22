using ECommerce.Domain.Entities.Commons;

namespace ECommerce.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        private string _name;
        private string _description;
        private string _brand;
        private string _color;
        private decimal _unitPrice;
        private double? _discount;
        private int _stock;
        public string Name
        {
            get => _name;
            set
            {

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ürün adı boş bırakılamaz.");

                if (value.Length > 100)
                    throw new ArgumentException("Ürün adı en fazla 100 karakter olabilir.");
                _name = value;


            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Açıklama boş bırakılamaz.");

                if (value.Length > 1000)
                    throw new ArgumentException("Açıklama en fazla 1000 karakter olabilir.");

                _description = value;
            }
        }
        public string Brand
        {
            get => _brand;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Marka boş bırakılamaz.");

                if (value.Length > 50)
                    throw new ArgumentException("Marka en fazla 50 karakter olabilir.");

                _brand = value;
            }
        }
        public string Color
        {
            get => _color;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Renk boş bırakılamaz.");

                _color = value;
            }
        }
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Birim fiyat negatif olamaz.");

                _unitPrice = value;
            }
        }
        public double? Discount
        {
            get => _discount;
            set
            {
                if (value != null && (value < 0 || value > 100))
                    throw new ArgumentException("İndirim yüzdesi 0 ile 100 arasında olmalıdır.");

                _discount = value;
            }
        }
        public decimal FinalPrice
        {
            get
            {
                if (Discount is null)
                    return UnitPrice;

                return UnitPrice - (UnitPrice * ((decimal)Discount.Value / 100));
            }
        }

        public int Stock
        {
            get => _stock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stok miktarı negatif olamaz.");

                _stock = value;
            }
        }

        public string? ImageUrl { get; set; }

        // Satışta olup olmadığını belirtir

        public bool Situation { get; set; } = true;

        // Nav Prop
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public Guid SellerId { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}