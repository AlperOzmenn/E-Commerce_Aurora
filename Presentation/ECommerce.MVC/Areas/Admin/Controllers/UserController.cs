using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(IAppUserService appUserService, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Kullanıcı Listesi
        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            int pageSize = 30;

            var users = await _appUserService.GetAllAsync();
            var userList = new List<AppUserListDTO>();

            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
                var roles = await _userManager.GetRolesAsync(appUser);

                var dto = _mapper.Map<AppUserListDTO>(user);
                dto.Role = roles.FirstOrDefault() ?? "Rol yok";
                userList.Add(dto);
            }

            var filteredUsers = userList.Filter(search, new Func<AppUserListDTO, string>[]
            {
                x => x.Name,
                x => x.Surname,
                x => x.Email,
                x => x.Role
            });

            var pagedUsers = filteredUsers.Paginate(pageNumber, pageSize).ToList();

            var viewModel = new ListViewModel<AppUserListDTO>
            {
                Items = pagedUsers,
                SearchQuery = search,
                PaginationInfo = new PaginationInfo
                {
                    Count = filteredUsers.Count(),
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                }
            };

            return View(viewModel);
        }

        // Kullanıcı Ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.Image != null && model.Image.Length > 0)
                {
                    string extension = Path.GetExtension(model.Image.FileName); // .jpg, .png vs
                    string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/userProfiles");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    model.ImagePath = uniqueFileName; 
                }

                var appUser = _mapper.Map<AppUser>(model);

                var result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Kullanıcı başarıyla oluşturuldu!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(model);
            }
        }

        //  Kullanıcı Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<AppUserUpdateDTO>(user);
            userDto.SelectedRole = userRoles.FirstOrDefault();

            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles);

            return View(userDto);
        }

        // Kullanıcı Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(AppUserUpdateDTO model)
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles);

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.SelectedRole))
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.SelectedRole);
                if (!roleExists)
                {
                    ModelState.AddModelError("SelectedRole", "Seçilen rol mevcut değil.");
                    return View(model);
                }

                var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }

            TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                TempData["Error"] = "Geçersiz kullanıcı ID'si!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var user = await _appUserService.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Kullanıcı Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _appUserService.SoftDeleteAsync(id);
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Silinmiş Kullanıcıları Listele
        [HttpGet]
        public async Task<IActionResult> Trash(string search, int pageNumber = 1)
        {
            try
            {
                int pageSize = 20;

                var deletedUsers = await _appUserService.GetAllDeletedAsync();

                if (!deletedUsers.Any())
                {
                    TempData["Info"] = "Henüz silinmiş bir ürün yok!";
                    return View(new ListViewModel<AppUserListDTO>
                    {
                        Items = new List<AppUserListDTO>(),
                        SearchQuery = search,
                        PaginationInfo = new PaginationInfo
                        {
                            Count = 0,
                            PageSize = pageSize,
                            CurrentPage = pageNumber
                        }
                    });
                }

                var userList = new List<AppUserListDTO>();

                foreach (var user in deletedUsers)
                {
                    var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
                    var roles = await _userManager.GetRolesAsync(appUser);

                    var dto = _mapper.Map<AppUserListDTO>(user);
                    dto.Role = roles.FirstOrDefault() ?? "Rol yok";
                    userList.Add(dto);
                }

                var filtered = userList.Filter(search, new Func<AppUserListDTO, string>[]
                {
                    x => x.Name,
                    x => x.Surname,
                    x => x.Email,
                    x => x.Role
                });

                var paged = filtered.Paginate(pageNumber, pageSize).ToList();

                var viewModel = new ListViewModel<AppUserListDTO>
                {
                    Items = paged,
                    SearchQuery = search,
                    PaginationInfo = new PaginationInfo
                    {
                        Count = filtered.Count(),
                        PageSize = pageSize,
                        CurrentPage = pageNumber
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silinmiş kullanıcılar yüklenemedi! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Kullanıcı Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            try
            {
                await _appUserService.RestoreAsync(id);
                TempData["Success"] = "Kullanıcı başarıyla geri yüklendi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Geri yükleme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Trash));
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            try
            {
                await _appUserService.DeleteAsync(id);
                TempData["Success"] = "Kullanıcı kalıcı olarak silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kalıcı silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Trash));
        }
    }
}