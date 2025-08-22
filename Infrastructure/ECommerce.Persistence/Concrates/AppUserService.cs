using AutoMapper;
using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistence.Concrates
{
    public class AppUserService : GenericService<AppUser, AppUserCreateDTO, AppUserUpdateDTO, AppUserListDTO, AppUserDTO>, IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepository<AppUser> _userRepository;

        public AppUserService(IRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager) : base(repository, unitOfWork, mapper)
        {
            _userManager = userManager;
            _userRepository = repository;
        }

        // Kullanıcıya rol verme
        public async Task AddUserToRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (!await _userManager.IsInRoleAsync(user, roleName))
                await _userManager.AddToRoleAsync(user, roleName);
        }

        // Kullanıcıdan rolü silme
        public async Task RemoveUserToRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (await _userManager.IsInRoleAsync(user,roleName))
                await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        // Kullanıcıya mesaj gönderme
        //public async Task SendMessageToUsserAsync(Guid userId, string message)
        //{
        //    var user = await _userRepository.GetByIdAsync(userId);

        //    if (user is null)
        //        throw new Exception("Kullanıcı bulunamadı!");
        //}
    }
}