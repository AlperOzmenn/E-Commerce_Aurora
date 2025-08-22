using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppUserService _appUserService;

        public UserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        // Kullanıcı Listesi
        public async Task<IActionResult> Index()
        {
            var users = await _appUserService.GetAllAsync();
            return View(users);
        }

        // Kullanıcı Ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Kullanıcı Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(AppUserCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(
                async () =>
                {
                    await _appUserService.AddAsync(model);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Kullanıcı başarılı bir şekilde eklendi!",
                errorMessage: "Kullanıcı ekleme işlemi sırasında bir hata oluştu!"
            );
        }

        // Kullanıcı Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    var userDto = await _appUserService.GetByIdAsync(id);

                    if (userDto == null)
                    {
                        TempData["Error"] = "Kullanıcı bulunamadı!";
                        return RedirectToAction(nameof(Index));
                    }

                    var updateDTO = new AppUserUpdateDTO
                    {
                        Name = userDto.Name,
                        Surname = userDto.Surname,
                        Email = userDto.Email,
                        Image = userDto.Image,
                        Password = default
                    };

                    return View(updateDTO);
                },
                errorMessage: "Kullanıcı bilgileri getirilirken hata oluştu!"
            );
        }

        // Kullanıcı Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(AppUserUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(
                async () =>
                {
                    await _appUserService.UpdateAsync(model);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Kullanıcı başarılı bir şekilde güncellendi!",
                errorMessage: "Güncelleme sırasında hata oluştu!"
            );
        }

        // Kullanıcı Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Geçersiz kullanıcı ID'si!";
                return RedirectToAction(nameof(Index));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var user = await _appUserService.GetByIdAsync(id);
                    if (user == null)
                    {
                        TempData["Error"] = "Kullanıcı bulunamadı!";
                        return RedirectToAction(nameof(Index));
                    }

                    return View(user);
                },
                errorMessage: "Detay getirme sırasında hata oluştu!"
            );
        }

        // Kullanıcı Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    await _appUserService.SoftDeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Kullanıcı başarıyla silindi!",
                errorMessage: "Silme işlemi sırasında hata oluştu!"
            );
        }

        // Silinmiş Kullanıcıları Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    var deletedUsers = await _appUserService.GetAllDeletedAsync();

                    if (!deletedUsers.Any())
                        TempData["Info"] = "Henüz silinmiş bir kullanıcı yok!";

                    return View(deletedUsers);
                },
                errorMessage: "Silinmiş kullanıcılar yüklenemedi!"
            );
        }

        // Kullanıcı Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    await _appUserService.RestoreAsync(id);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Kullanıcı başarıyla geri yüklendi!",
                errorMessage: "Geri yükleme işlemi sırasında hata oluştu!"
            );
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    await _appUserService.DeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Kullanıcı kalıcı olarak silindi!",
                errorMessage: "Kalıcı silme sırasında hata oluştu!"
            );
        }
    }
}
