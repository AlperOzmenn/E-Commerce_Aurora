using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.AppUserDTOs
{
    public record AppUserDetailsDTO : BaseDTO
    {
        [Display(Name = "İsim")]
        public string Name { get; init; }

        [Display(Name = "Soyisim")]
        public string Surname { get; init; }

        [Display(Name = "Email")]
        public string Email { get; init; }

        [Display(Name = "Kullanıcı Fotoğrafı")]
        public string? Image { get; init; }

        public DateTime CreatedDate { get; init; }
        public DateTime? UpdatedDate { get; init; }
        public DateTime? DeletedDate { get; init; }
        public bool IsDeleted { get; init; } = false;
    }
}
