using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.UserDTOs
{
    public record AppUserCreateDTO : BaseDTO
    {
        [Required(ErrorMessage = "İsim alanı boş geçilemez!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "İsim 3 ile 20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "İsim sadece harf ve boşluk içerebilir.")]
        [Display(Name = "İsim")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Soyisim 3 ile 20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Soyisim sadece harf ve boşluk içerebilir.")]
        [Display(Name = "Soyisim")]
        public string Surname { get; init; }

        [Required(ErrorMessage = "E-mail alanı boş geçilemez!")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail adresi giriniz.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "E-mail '@' ve geçerli bir uzantı içermelidir.")]
        [Display(Name = "E-mail")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Parola alanı boş geçilemez!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
           // ErrorMessage = "Parola en az 1 büyük harf, 1 küçük harf ve 1 rakam içermelidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; init; }

        [Display(Name = "Kullanıcı Fotoğrafı")]
        [DataType(DataType.Upload)]
        public IFormFile? Image { get; init; }

        public string? ImagePath { get; set; }
    }
}