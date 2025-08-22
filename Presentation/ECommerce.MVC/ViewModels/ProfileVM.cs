namespace ECommerce.MVC.ViewModels
{
    public class ProfileVM
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "/images/userProfile/defautProfile.png";
    }
}
