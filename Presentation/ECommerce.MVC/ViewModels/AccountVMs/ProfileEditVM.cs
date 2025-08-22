using System.ComponentModel.DataAnnotations;

namespace ECommerce.MVC.ViewModels.AccountVMs
{
    public class ProfileEditVM
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public IFormFile? Image { get; set; }  // Profil fotoğrafı yükleme
        public string? ImagePath { get; set; }  // Profil fotoğrafının mevcut yolu (Varsa)
    }
}
