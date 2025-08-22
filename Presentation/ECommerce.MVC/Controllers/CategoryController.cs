using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.UnitOfWorks;
using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.MVC.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ICategoryService categoryService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        // Kategori Listesi
        public async Task<IActionResult> Index()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var categories = await _categoryService.GetAllAsync();
                return View(categories);
            }, errorMessage: "Kategori listesi yüklenirken hata oluştu!");
        }

        // Kategori Ekleme (GET)
        [HttpGet]
        public IActionResult Create() => View();

        // Kategori Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
                await _categoryService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kategori başarılı bir şekilde eklendi!", errorMessage: "Kategori ekleme işlemi sırasında hata oluştu!");
        }

        // Kategori Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (id == Guid.Empty)
                {
                    TempData["Error"] = "Geçersiz kategori ID'si!";
                    return RedirectToAction(nameof(Index));
                }

                var categoryDto = await _categoryService.GetByIdAsync(id);
                if (categoryDto == null)
                {
                    TempData["Error"] = "Kategori bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                var updateDTO = new CategoryUpdateDTO
                {
                    Id = categoryDto.Id,
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };

                return View(updateDTO);
            }, errorMessage: "Kategori bilgileri getirilirken hata oluştu!");
        }

        // Kategori Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
                await _categoryService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kategori başarılı bir şekilde güncellendi!", errorMessage: "Güncelleme sırasında hata oluştu!");
        }

        // Kategori Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (id == Guid.Empty)
                {
                    TempData["Error"] = "Geçersiz kategori ID'si!";
                    return RedirectToAction(nameof(Index));
                }

                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "Kategori bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }, errorMessage: "Detay getirme sırasında hata oluştu!");
        }

        // Kategori Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _categoryService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kategori başarıyla silindi!", errorMessage: "Silme işlemi sırasında hata oluştu!");
        }

        // Silinmiş Kategorileri Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var deletedCategories = await _categoryService.GetAllDeletedAsync();
                if (!deletedCategories.Any())
                    TempData["Info"] = "Henüz silinmiş bir kategori yok!";

                return View(deletedCategories);
            }, errorMessage: "Silinmiş kategoriler yüklenemedi!");
        }

        // Kategori Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _categoryService.RestoreAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kategori başarıyla geri yüklendi!", errorMessage: "Geri yükleme işlemi sırasında hata oluştu!");
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _categoryService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kategori kalıcı olarak silindi!", errorMessage: "Kalıcı silme sırasında hata oluştu!");
        }

        public async Task<IActionResult> GetProductByCategory(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (id == Guid.Empty)
                {
                    TempData["Error"] = "Geçersiz kategori ID'si!";
                    return RedirectToAction(nameof(Index));
                }

                var products = await _unitOfWork.GetRepository<Product>().GetFilteredListAsync(
                    select: p => new ProductListDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        UnitPrice = p.UnitPrice,
                        CategoryName = p.Category.Name,
                        Brand = p.Brand,
                        Discount = p.Discount,
                        FinalPrice = p.FinalPrice,
                        ImagePath = p.ImageUrl,
                        Stock = p.Stock,
                        Description = p.Description,
                    },
                    where: p => p.CategoryId == id && !p.IsDeleted,
                    join: p => p.Include(x => x.Category)
                );

                if (!products.Any())
                {
                    TempData["Info"] = "Bu kategoriye ait ürün bulunamadı!";
                }

                return View(products);
            }, errorMessage: "Ürünler yüklenirken hata oluştu!");
        }
    }
}
