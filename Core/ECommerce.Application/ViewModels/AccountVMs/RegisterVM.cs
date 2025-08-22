using ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.ViewModels.AccountVMs
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "İsim boş geçilemez!")]
        [Display(Name = "İsim: ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim boş geçilemez!")]
        [Display(Name = "Soyisim: ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [EmailAddress(ErrorMessage = "Email adresi formatına uygun bir giriş yapınız!")]
        [Display(Name = "E-Mail: ")]
        public string Email { get; set; }

        [Display(Name = "Cinsiyet")]
        public GenderEnum? Gender { get; init; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [Display(Name = "Şifre: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar: ")]
        [Compare("Password", ErrorMessage = "Girilen şifreler tutarsız!")]
        public string ConfirmPassword { get; set; }
    }
}