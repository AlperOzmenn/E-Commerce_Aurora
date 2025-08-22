using ECommerce.Domain.Enums.PaymentEnums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.PaymentDTOs
{
    public class PaymentDTO
    {
        [Display(Name = "Ödeme ID")]
        public Guid Id { get; set; }

        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Durum")]
        public PaymentStatus Status { get; set; }

        [Display(Name = "İşlem Numarası")]
        public string TransactionId { get; set; }

        [Display(Name = "Yöntem")]
        public PaymentMethod Method { get; set; }

        [Display(Name = "Kart Sahibi")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma Ayı")]
        public string ExpirationMonth { get; set; }

        [Display(Name = "Son Kullanma Yılı")]
        public string ExpirationYear { get; set; }

        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Sipariş Detay ID")]
        public Guid OrderDetailId { get; set; }
        public Guid AppUserId { get; set; }
    }
}
