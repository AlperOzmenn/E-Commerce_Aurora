using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IAppUserService : IGenericService<AppUser, AppUserCreateDTO, AppUserUpdateDTO, AppUserListDTO, AppUserDTO>
    {
        // Kullanıcıya rol ekleme
        Task AddUserToRoleAsync(Guid userId, string roleName);

        // Kullanıcıdan rolü silme
        Task RemoveUserToRoleAsync(Guid userId, string roleName);

        //Kullanıcıya mesaj gönderme
        //Task SendMessageToUsserAsync(Guid userId, string message);
    }
}
