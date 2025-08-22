using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.SellerDTOs
{
    public record SellerDTO : BaseDTO 
    {
        //[Display(Name = "Firma Adı")]
        //[StringLength(100, ErrorMessage = "Firma adı en fazla 100 karakter olabilir.")]
        //public string CompanyName { get; init; }

        //[Display(Name = "İletişim Kişisi")]
        //[StringLength(100, ErrorMessage = "İletişim kişisi en fazla 100 karakter olabilir.")]
        //public string ContactName { get; init; }

        //[Display(Name = "Yetki/Unvan")]
        //[StringLength(50, ErrorMessage = "Unvan en fazla 50 karakter olabilir.")]
        //public string ContactTitle { get; init; }

        //[Display(Name = "Oluşturulma Tarihi")]
        //[DataType(DataType.DateTime)]
        //public DateTime CreatedDate { get; init; }

        //[Display(Name = "Güncellenme Tarihi")]
        //[DataType(DataType.DateTime)]
        //public DateTime? UpdatedDate { get; init; }

        //[Display(Name = "Silinme Tarihi")]
        //[DataType(DataType.DateTime)]
        //public DateTime? DeletedDate { get; init; }

        //[Display(Name = "Silinmiş Mi?")]
        //public bool IsDeleted { get; init; } = false;


        // Seller bilgileri
        [Required(ErrorMessage = "Firma adı boş geçilemez.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Firma adı 3 ile 100 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9ğüşöçİĞÜŞÖÇ\s\.\-&]+$", ErrorMessage = "Firma adı yalnızca harf, rakam, boşluk ve (.,-,&) karakterlerini içerebilir.")]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; init; }

        [Required(ErrorMessage = "İletişim kişisi adı boş geçilemez.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "İletişim kişisi adı 3 ile 50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "İletişim kişisi yalnızca harf ve boşluk içerebilir.")]
        [Display(Name = "İletişim Kişisi")]
        public string ContactName { get; init; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Unvan en az 2, en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Unvan yalnızca harf ve boşluk içermelidir.")]
        [Display(Name = "Yetki / Unvan")]
        public string ContactTitle { get; init; }

        [Display(Name = "Onay Durumu")]
        public bool Approved { get; init; }


        // AppUser bilgileri
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ad 2 ile 30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Ad yalnızca harf ve boşluk içermelidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Kullanıcı soyadı boş geçilemez.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Soyad 2 ile 30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Soyad yalnızca harf ve boşluk içermelidir.")]
        [Display(Name = "Kullanıcı Soyadı")]
        public string Surname { get; init; }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [StringLength(100, ErrorMessage = "Email adresi en fazla 100 karakter olabilir.")]
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