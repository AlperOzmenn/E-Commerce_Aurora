using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Enums.PaymentEnums;

namespace ECommerce.Domain.Entities.Payments
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; } // iyzico / stripe vs'den dönen işlem ID

        public PaymentMethod Method { get; set; } // Kredi kartı, havale, vs.

        // Kredi kartı bilgileri
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string ExpirationMonth { get; set; }

        public string ExpirationYear { get; set; }

        public string CVV { get; set; }

        // Navigation properties
        public Guid OrderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }

        public Guid AppUserId { get; set; }  
        public AppUser AppUser { get; set; }
    }
}