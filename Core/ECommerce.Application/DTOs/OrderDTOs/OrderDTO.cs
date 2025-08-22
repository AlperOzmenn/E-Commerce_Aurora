using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public record OrderDTO : BaseDTO
    {
        [Required(ErrorMessage = "Sipariş numarası zorunludur.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Sipariş numarası 5 ile 20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Sipariş numarası yalnızca harf, rakam ve tire içerebilir.")]
        [Display(Name = "Sipariş Numarası")]
        public string OrderNumber { get; init; }

        [Display(Name = "Sipariş Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; init; }

        [Display(Name = "Toplam Tutar")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Toplam tutar negatif olamaz.")]
        public decimal TotalAmount { get; init; }

        [Required(ErrorMessage = "Müşteri adı boş bırakılamaz.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Müşteri adı 3 ile 50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Müşteri adı sadece harf ve boşluk içermelidir.")]
        [Display(Name = "Müşteri Adı")]
        public string CustomerName { get; init; }

        [Display(Name = "Sipariş Durumu")]
        public OrderStatus Status { get; init; } = OrderStatus.Pending;

        [Display(Name = "Ürünler")]
        public virtual ICollection<Product> Products { get; init; } = new List<Product>();
    }
}
