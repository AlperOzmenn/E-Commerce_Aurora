using ECommerce.Application.DTOs.AppUserDTOs;

namespace ECommerce.MVC.ViewModels.AdminVMs
{
    public class AppUserListVM
    {
        public List<AppUserListDTO> Users { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public string SearchQuery { get; set; }
    }
}