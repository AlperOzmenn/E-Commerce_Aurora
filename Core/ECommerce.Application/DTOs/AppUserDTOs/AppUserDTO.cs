using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.AppUserDTOs
{
    public record AppUserDTO : BaseDTO
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

        [Display(Name = "Kullanıcı Fotoğrafı")]
        public string? Image { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? UpdatedDate { get; init; }
        public DateTime? DeletedDate { get; init; }
        public bool IsDeleted { get; init; } = false;
        public GenderEnum? Gender { get; init; }
    }
}
