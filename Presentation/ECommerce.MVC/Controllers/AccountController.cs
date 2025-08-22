using AutoMapper;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.ViewModels.AccountVMs;
using ECommerce.Domain.Entities;
using ECommerce.MVC.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ISellerService _sellerService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ISellerService sellerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _sellerService = sellerService;
        }

        // Giriş Yap (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Giriş Yap (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya parola yanlış!");
                return View(model);
            }

            if (user.IsDeleted)
            {
                // Soft delete edilmiş kullanıcıyı uyarı sayfasına gönder
                return RedirectToAction(nameof(Suspended));
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya parola yanlış!");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "Admin", new { area = "Admin" });

            if (roles.Contains("Seller"))
                return RedirectToAction("Index", "Product", new { area = "Seller" });

            //if (roles.Contains("Customer"))
            //    return RedirectToAction("Index", "Customer", new { area = "Customer" });

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ProfileEdit()
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı al

            // Kullanıcıyı ProfileEditVM'e dönüştür
            var viewModel = new ProfileEditVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,
                ImagePath = user.Image // Eğer bir profil fotoğrafı varsa, yolunu alıyoruz
            };

            return View(viewModel);  // Profil düzenleme sayfasını gönderiyoruz
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ProfileEditVM model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);  // Eğer model geçerli değilse, formu tekrar göster

            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı al

            if (user == null)
            {
                return RedirectToAction("Login", "Account");  // Eğer kullanıcı bulunamazsa, login sayfasına yönlendir
            }

            // Profil bilgilerini güncelle
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.UserName;

            // Eğer yeni bir profil fotoğrafı yüklenmişse
            if (model.Image != null)
            {
                // Yüklenen fotoğrafı kaydet
                var fileName = Path.GetFileName(model.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);  // Fotoğrafı kaydediyoruz
                }

                // Fotoğrafın yolunu kaydediyoruz
                user.Image = "/images/" + fileName;
            }

            // Kullanıcıyı güncelle
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profiliniz başarıyla güncellendi!";
                return RedirectToAction("Profile", "Account"); // Profil sayfasına yönlendir
            }

            // Hata varsa, kullanıcıya hata mesajı göster
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);  // Eğer güncelleme başarısızsa, formu tekrar gösteriyoruz
        }


        // Soft delete olmuş kullanıcılar için uyarı sayfası
        [HttpGet]
        public IActionResult Suspended()
        {
            return View(); // Suspended.cshtml olacak, View klasöründe hazır olmalı
        }

        // Çıkış Yap (GET)
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Kayıt Ol (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Kayıt Ol (POST)
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // AutoMapper ile RegisterVM'den AppUser oluştur
            var user = _mapper.Map<AppUser>(model);

            // Identity için UserName zorunlu, genelde Email ile set edilir
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Kullanıcı kayıt olduğu zaman customer rolünü alır.
                await _userManager.AddToRoleAsync(user, "Customer");

                var loginResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (loginResult.Succeeded)
                    return RedirectToAction("Index", "Home");

                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);  // model ile dön ki form verileri kalsın ve validasyonlar gözüksün
        }

        // Profile Görüntüleme (GET)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı al

            var viewModel = new ProfileVM
            {

                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,
                Image = user.Image

            };

            return View(viewModel);
        }

        // Satıcı Kayıt (GET)
        [HttpGet]
        public IActionResult SellerRegister()
        {
            return View();
        }

        // Satıcı Kayıt (POST)
        [HttpPost]
        public async Task<IActionResult> SellerRegister(SellerRegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1. Kullanıcı oluştur
            var user = new AppUser
            {
                Name = model.Name,
                Surname = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // 2. Default olarak Customer rolünü ata
            await _userManager.AddToRoleAsync(user, "Customer");

            // 3. SellerCreateDTO oluştur ve doldur
            var sellerCreateDto = new SellerCreateDTO
            {
                AppUserId = user.Id,  // Eğer SellerCreateDTO içinde AppUserId yoksa, eklemelisin
                CompanyName = model.CompanyName,
                ContactName = model.ContactName,
                ContactTitle = model.ContactTitle,
                Approved = false      // Admin onayına düşecek
            };

            // 4. Servise SellerCreateDTO gönder
            await _sellerService.AddAsync(sellerCreateDto);

            // 5. Otomatik login isteğe bağlı
            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["Success"] = "Satıcı başvurunuz alınmıştır. Onaylanınca bilgilendirileceksiniz.";
            return RedirectToAction("Index", "Home");
        }
    }
}