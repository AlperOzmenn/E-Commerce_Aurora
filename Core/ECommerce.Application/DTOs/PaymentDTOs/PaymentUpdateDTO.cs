using ECommerce.Domain.Enums.PaymentEnums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.PaymentDTOs
{
    public class PaymentUpdateDTO
    {
        [Required(ErrorMessage = "ID bilgisi zorunludur.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tutar alanı zorunludur.")]
        [Display(Name = "Ödeme Tutarı")]
        public decimal Amount { get; set; }

        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ödeme durumu belirtilmelidir.")]
        [Display(Name = "Ödeme Durumu")]
        public PaymentStatus Status { get; set; }

        [Required(ErrorMessage = "İşlem numarası zorunludur.")]
        [Display(Name = "İşlem Numarası")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi seçilmelidir.")]
        [Display(Name = "Ödeme Yöntemi")]
        public PaymentMethod Method { get; set; }

        [Required(ErrorMessage = "Kart üzerindeki isim girilmelidir.")]
        [Display(Name = "Kart Sahibi")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [CreditCard(ErrorMessage = "Geçerli bir kart numarası giriniz.")]
        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Son kullanma ayı girilmelidir.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Ay 2 haneli olmalıdır.")]
        [Display(Name = "Son Kullanma Ayı")]
        public string ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Son kullanma yılı girilmelidir.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Yıl 4 haneli olmalıdır.")]
        [Display(Name = "Son Kullanma Yılı")]
        public string ExpirationYear { get; set; }

        [Required(ErrorMessage = "CVV kodu zorunludur.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV 3 haneli olmalıdır.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "İlgili sipariş detayı seçilmelidir.")]
        [Display(Name = "Sipariş Detay ID")]
        public Guid OrderDetailId { get; set; }
    }
}
