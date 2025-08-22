using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public record OrderUpdateDTO : BaseDTO
    {
        [Display(Name = "Sipariş Durumu")]
        public OrderStatus Status { get; init; } = OrderStatus.Pending;
    }
}
