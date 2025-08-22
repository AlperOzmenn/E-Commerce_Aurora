using System.ComponentModel.DataAnnotations;

namespace ECommerce.MVC.ViewModels.AccountVMs
{
    public class ProfileEditVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
