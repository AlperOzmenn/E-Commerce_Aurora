using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.SellerDTOs
{
    public record SellerDetailDTO : BaseDTO
    {
        // Kullanıcı bilgileri
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // Firma bilgileri
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }

        [Display(Name = "İletişim Kişisi")]
        public string ContactName { get; set; }

        [Display(Name = "Yetki / Unvan")]
        public string ContactTitle { get; set; }

        [Display(Name = "Onaylı mı?")]
        public bool Approved { get; set; }

        // Ortak zamanlar
        [Display(Name = "Oluşturulma")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Güncellenme")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Silinme")]
        public DateTime? DeletedDate { get; set; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; set; }
    }
}
