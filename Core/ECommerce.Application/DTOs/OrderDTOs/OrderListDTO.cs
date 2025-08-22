using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public record OrderListDTO : BaseDTO
    {
        [Display(Name = "Sipariş Numarası")]
        public string OrderNumber { get; init; }

        [Display(Name = "Sipariş Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; init; }

        [Display(Name = "Toplam Tutar")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; init; }

        [Display(Name = "Müşteri Adı")]
        [StringLength(100, ErrorMessage = "Müşteri adı en fazla 100 karakter olabilir.")]
        public string CustomerName { get; init; }

        [Display(Name = "Sipariş Durumu")]
        public OrderStatus Status { get; init; } = OrderStatus.Pending;
        // Additional properties can be added as needed
    }
}
