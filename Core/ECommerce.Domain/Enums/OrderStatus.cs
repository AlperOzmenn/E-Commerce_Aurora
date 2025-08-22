
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Beklemede")]
        Pending,

        [Display(Name = "Onaylandı")]
        Approved,

        [Display(Name = "Kargoya Verildi")]
        Shipped,

        [Display(Name = "Teslim Edildi")]
        Delivered,

        [Display(Name = "İptal Edildi")]
        Cancelled
    }
}
