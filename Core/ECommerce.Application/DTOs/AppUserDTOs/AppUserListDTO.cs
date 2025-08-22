using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.AppUserDTOs
{
    public record AppUserListDTO : BaseDTO
    {
        [Display(Name = "İsim")]
        public string Name { get; init; }

        [Display(Name = "Soyisim")]
        public string Surname { get; init; }

        [Display(Name = "E-mail")]
        public string Email { get; init; }

        [Display(Name = "Kullanıcı Fotoğrafı")]
        public string? Image { get; init; }

        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; init; }


        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedDate { get; init; }


        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; init; }


        [Display(Name = "Silindi Mi")]
        public bool IsDeleted { get; init; } = false;
    }
}