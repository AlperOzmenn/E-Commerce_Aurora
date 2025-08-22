using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.SellerDTOs
{
    public record SellerUpdateDTO : BaseDTO
    {
        [Required(ErrorMessage = "Firma adı zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Firma adı 3 ile 100 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9ğüşöçİĞÜŞÖÇ\s\.\-&]+$", ErrorMessage = "Firma adı yalnızca harf, rakam, boşluk ve (.,-,&) karakterlerini içerebilir.")]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; init; }

        [Required(ErrorMessage = "İlgili kişi adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "İlgili kişi adı 3 ile 50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "İlgili kişi adı yalnızca harf ve boşluk içermelidir.")]
        [Display(Name = "İletişim Kişisi")]
        public string ContactName { get; init; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Unvan en az 2, en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Unvan yalnızca harf ve boşluk içermelidir.")]
        [Display(Name = "Yetki/Unvan")]
        public string ContactTitle { get; init; }

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdateDate { get; init; }
    }
}
