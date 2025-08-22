using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public record OrderDetailDTO : BaseDTO
    {

        
        [Display(Name = "Sipariş Numarası")]
        public string OrderNumber { get; init; }

        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; init; }

        [Display(Name = "Toplam Tutar")]
        public decimal TotalAmount { get; init; }
 
        [Display(Name = "Müşteri Adı")]
        public string CustomerName { get; init; }

        [Display(Name = "Sipariş Durumu")]
        public OrderStatus Status { get; init; } = OrderStatus.Pending;

        [Display(Name = "Sipariş Ürünleri")]
        public virtual ICollection<Product> Products { get; init; } = new List<Product>();
    }
}
