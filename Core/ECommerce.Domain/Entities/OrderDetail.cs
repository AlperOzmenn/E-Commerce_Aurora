using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        private int _quantity;
        private decimal _unitPrice;
        private double _discount;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Ürün adedi en az 1 olmalıdır.");
                _quantity = value;
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
        public double Discount
        {
            get => _discount;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("İndirim oranı 0 ile 100 arasında olmalıdır.");
                _discount = value;
            }
        }
        public decimal TotalPrice { get; set; }
        // public decimal TotalPrice => UnitPrice * Quantity * (decimal)(1 - Discount / 100);

        //nav properties
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        //public Guid PaymentId { get; set; }
        //public Payment Payment { get; set; }
        



    }
}