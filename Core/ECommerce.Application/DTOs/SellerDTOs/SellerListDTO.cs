using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.SellerDTOs
{
    public record SellerListDTO : BaseDTO
    {
        // Seller bilgileri
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; init; }

        [Display(Name = "İletişim Kişisi")]
        public string ContactName { get; init; }

        [Display(Name = "Yetki / Unvan")]
        public string ContactTitle { get; init; }

        [Display(Name = "Onay Durumu")]
        public bool Approved { get; init; }

        // AppUser bilgileri
        [Display(Name = "Kullanıcı Adı")]
        public string Name { get; init; }

        [Display(Name = "Kullanıcı Soyadı")]
        public string Surname { get; init; }

        [Display(Name = "Email Adresi")]
        public string Email { get; init; }

        // Ortak bilgiler
        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedDate { get; init; }

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedDate { get; init; }

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; init; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; init; }
    }
}