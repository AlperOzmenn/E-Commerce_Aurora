using ECommerce.Domain.Enums.PaymentEnums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.PaymentDTOs
{
    public class PaymentListDTO
    {
        [Display(Name = "Ödeme ID")]
        public Guid Id { get; set; }

        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Durum")]
        public PaymentStatus Status { get; set; }

        [Display(Name = "Yöntem")]
        public PaymentMethod Method { get; set; }

        [Display(Name = "İşlem Numarası")]
        public string TransactionId { get; set; }

        [Display(Name = "Sipariş Detay ID")]
        public Guid OrderDetailId { get; set; }
    }
}
