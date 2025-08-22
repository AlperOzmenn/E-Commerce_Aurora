using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.MVC.Controllers;
using ECommerce.Persistence.Concrates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class ProductController : BaseController
    {
        #region MyRegion
        //private readonly IProductService _productService;

        //public ProductController(IProductService productService)
        //{
        //    _productService = productService;
        //}

        //// Kullanıcı Listesi
        //public async Task<IActionResult> Index()
        //{
        //    var products = await _productService.GetAllAsync();
        //    return View(products);
        //}

        //// Kullanıcı Ekleme (GET)
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// Kullanıcı Ekleme (POST)
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductCreateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    try
        //    {
        //        await _productService.AddAsync(model);
        //        TempData["Success"] = "Kullanıcı başarılı bir şekilde eklendi!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Kullanıcı ekleme işlemi sırasında bir hata oluştu! Hata: {ex.Message}";
        //        return View(model);
        //    }
        //}

        //// Kullanıcı Güncelleme (GET)
        //[HttpGet]
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    try
        //    {
        //        var productDto = await _productService.GetByIdAsync(id);

        //        ProductUpdateDTO updateDTO = new ProductUpdateDTO
        //        {
        //            Id = productDto.Id,
        //            Name = productDto.Name,
        //            Description = productDto.Description,
        //            Price = productDto.Price,
        //            Stock = productDto.Stock,
        //        };

        //        if (productDto == null)
        //        {
        //            TempData["Error"] = "Kullanıcı bulunamadı!";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        return View(updateDTO);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Kullanıcı bilgileri getirilirken hata oluştu! Hata: {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// Kullanıcı Güncelleme (POST)
        //[HttpPost]
        //public async Task<IActionResult> Edit(ProductUpdateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    try
        //    {
        //        await _productService.UpdateAsync(model);
        //        TempData["Success"] = "Kullanıcı başarılı bir şekilde güncellendi!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Güncelleme sırasında hata oluştu! Hata: {ex.Message}";
        //        return View(model);
        //    }
        //}

        //// Kullanıcı Detayı
        //[HttpGet]
        //public async Task<IActionResult> Detail(Guid id)
        //{
        //    if (string.IsNullOrWhiteSpace(id.ToString()))
        //    {
        //        TempData["Error"] = "Geçersiz kullanıcı ID'si!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    try
        //    {
        //        var product = await _productService.GetByIdAsync(id);
        //        if (product == null)
        //        {
        //            TempData["Error"] = "Kullanıcı bulunamadı!";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        return View(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// Kullanıcı Soft Delete
        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        await _productService.SoftDeleteAsync(id);
        //        TempData["Success"] = "Kullanıcı başarıyla silindi!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Silme işlemi sırasında hata oluştu! Hata: {ex.Message}";
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //// Silinmiş Kullanıcıları Listele
        //[HttpGet]
        //public async Task<IActionResult> Trash()
        //{
        //    try
        //    {
        //        var deletedProducts = await _productService.GetAllDeletedAsync();

        //        if (!deletedProducts.Any())
        //            TempData["Info"] = "Henüz silinmiş bir kullanıcı yok!";

        //        return View(deletedProducts);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Silinmiş kullanıcılar yüklenemedi! Hata: {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// Kullanıcı Restore Etme
        //[HttpGet]
        //public async Task<IActionResult> Restore(Guid id)
        //{
        //    try
        //    {
        //        await _productService.RestoreAsync(id);
        //        TempData["Success"] = "Kullanıcı başarıyla geri yüklendi!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Geri yükleme işlemi sırasında hata oluştu! Hata: {ex.Message}";
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
        #endregion
        private readonly ISellerService _sellerService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, IMapper mapper, ISellerService sellerService,ICategoryService categoryService,UserManager<AppUser> userManager,ICommentService commentService)
        {
            _productService = productService;
            _mapper = mapper;
            _sellerService = sellerService;
            _categoryService = categoryService;
            _userManager = userManager;
            _commentService = commentService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdStr, out Guid userId))
                return userId;

            throw new UnauthorizedAccessException("Kullanıcı ID'si alınamadı.");
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Guid userId = GetCurrentUserId();

                // Metod adı doğru yazıldı:
                Guid sellerId = await _sellerService.GetSellerIdByUserIdAsync(userId);

                var products = await _productService.GetProductsBySellerIdAsync(sellerId);

                var productDTOs = await _productService.GetAllWithCategoryAsync();

                return View(productDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Oturum açmanız gerekiyor.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Ürün Ekleme (GET)
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _categoryService.GetAllAsync(); // DTO değilse model mapping yapılmalı
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        // Ürün Ekleme(POST)
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            try
            {
                // Görsel varsa kaydet ve DTO'da sakla
                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    string extension = Path.GetExtension(model.ImageUrl.FileName); // .jpg, .png vs
                    string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productsImages");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUrl.CopyToAsync(stream);
                    }

                    model.ImagePath = uniqueFileName; // AutoMapper'a gönderiyoruz bu değeri
                }

                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                var user = await _userManager.GetUserAsync(User);
                var seller = await _sellerService.GetSellerIdByUserIdAsync(user.Id);
                model.SellerId = seller;

                await _productService.AddAsync(model);
                TempData["Success"] = "Ürün başarılı bir şekilde eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün ekleme sırasında hata oluştu: {ex.Message}";

                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                return View(model);
            }
        }

        //// Ürün Ekleme(POST)
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductCreateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        return View(model);
        //    }

        //    try
        //    {
        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        var user = await _userManager.GetUserAsync(User);
        //        var seller = await _sellerService.GetSellerIdByUserIdAsync(user.Id);
        //        model.SellerId = seller;

        //        string uniqueFileName = null;

        //        if (model.ImageUrl != null)
        //        {
        //            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productsImages");
        //            Directory.CreateDirectory(uploadsFolder); // Klasör yoksa oluşturacak.

        //            string extension = Path.GetExtension(model.ImageUrl.FileName);
        //            uniqueFileName = $"{Guid.NewGuid()}{extension}";
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                model.ImageUrl.CopyTo(stream);
        //            }
        //        }

        //        await _productService.AddAsync(model);
        //        TempData["Success"] = "Ürün başarılı bir şekilde eklendi!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Ürün ekleme sırasında hata oluştu: {ex.Message}";

        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        //        return View(model);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(ProductCreateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        return View(model);
        //    }

        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        var seller = await _sellerService.GetSellerIdByUserIdAsync(user.Id);
        //        model.SellerId = seller;

        //         Fotoğraf işlemi
        //        if (model.ImageUrl != null && model.ImageUrl.Length > 0)
        //        {
        //            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productsImages");
        //            Directory.CreateDirectory(uploadsFolder); // Klasör yoksa oluştur

        //            string extension = Path.GetExtension(model.ImageUrl.FileName);
        //            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await model.ImageUrl.CopyToAsync(stream);
        //            }

        //            model.ImagePath = $"/images/productsImages/{uniqueFileName}";
        //        }

        //        await _productService.AddAsync(model);

        //        TempData["Success"] = "Ürün başarılı bir şekilde eklendi!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Ürün ekleme sırasında hata oluştu: {ex.Message}";

        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        //        return View(model);
        //    }
        //}

        // Ürün Ekleme (POST)
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductCreateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        return View(model);
        //    }

        //    try
        //    {
        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        //        //var user = await _userManager.GetUserAsync(User);
        //        //if (user == null)
        //        //{
        //        //    TempData["Error"] = "Kullanıcı bilgisi alınamadı!";
        //        //    return RedirectToAction("Login", "Account");
        //        //}

        //        //model.SellerId = user.Id; // ✅ SellerId burada atanıyor
        //        var user = await _userManager.GetUserAsync(User);
        //        var seller = await _sellerService.GetSellerIdByUserIdAsync(user.Id);
        //        model.SellerId = seller;
        //        await _productService.AddAsync(model);

        //        TempData["Success"] = "Ürün başarılı bir şekilde eklendi!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Ürün ekleme sırasında hata oluştu: {ex.Message}";

        //        var categories = await _categoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        //        return View(model);
        //    }
        //}

        // Ürün Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var productDto = await _productService.GetByIdAsync(id);

                ProductUpdateDTO updateDTO = new ProductUpdateDTO
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Stock = productDto.Stock,
                };

                if (productDto == null)
                {
                    TempData["Error"] = "Ürün bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(updateDTO);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün bilgileri getirilirken hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Ürün Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _productService.UpdateAsync(model);
                TempData["Success"] = "Ürün başarılı bir şekilde güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

        //// Ürün Detayı
        //[HttpGet]
        //public async Task<IActionResult> Detail(Guid id)
        //{
        //    if (string.IsNullOrWhiteSpace(id.ToString()))
        //    {
        //        TempData["Error"] = "Geçersiz ürün ID'si!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    try
        //    {
        //        var product = await _productService.GetByIdAsync(id);
        //        if (product == null)
        //        {
        //            TempData["Error"] = "Ürün bulunamadı!";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        return View(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        // Ürün Detayı

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                TempData["Error"] = "Geçersiz ürün ID'si!";
                return RedirectToAction(nameof(Index));
            }

            return await ExecuteSafeAsync(
                async () =>
                {
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                    {
                        TempData["Error"] = "Ürün bulunamadı!";
                        return RedirectToAction(nameof(Index));
                    }

                    var comments = await _commentService.GetCommentsByProductIdAsync(id);
                    foreach (var comment in comments)
                    {
                        comment.Replies = comments
                            .Where(c => c.ParentCommentId == comment.Id)
                            .ToList();
                    }

                    var viewModel = new ProductDetailVM
                    {
                        Product = product,
                        Comments = comments
                    };

                    return View(viewModel);
                },
                errorMessage: "Ürün detayı getirilirken bir hata oluştu!"
            );
        }

        // Ürün Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.SoftDeleteAsync(id);
                TempData["Success"] = "Ürün başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Silinmiş Ürünleri Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            try
            {
                var deletedProducts = await _productService.GetAllDeletedAsync();

                if (!deletedProducts.Any())
                    TempData["Info"] = "Henüz silinmiş bir ürün yok!";

                return View(deletedProducts);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silinmiş ürünler yüklenemedi! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Ürün Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            try
            {
                await _productService.RestoreAsync(id);
                TempData["Success"] = "Ürün başarıyla geri yüklendi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Geri yükleme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                TempData["Success"] = "Ürün kalıcı olarak silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kalıcı silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
