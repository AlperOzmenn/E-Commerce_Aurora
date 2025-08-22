using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // Kategori Listesi
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            var categories = await _categoryService.GetAllAsync();

            var filtered = categories.Filter(search, new Func<CategoryListDTO, string>[]
            {
                x => x.Name,
                x => x.Description ?? string.Empty
            });

            var paged = filtered.Paginate(pageNumber, pageSize).ToList();

            var viewModel = new ListViewModel<CategoryListDTO>
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


        // Kategori Ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Kategori Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _categoryService.AddAsync(model);
                TempData["Success"] = "Kategori başarılı bir şekilde eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kategori ekleme işlemi sırasında bir hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

        // Kategori Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var categoryDto = await _categoryService.GetByIdAsync(id);

                CategoryUpdateDTO updateDTO = new CategoryUpdateDTO
                {
                    Id = categoryDto.Id,
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };

                if (categoryDto == null)
                {
                    TempData["Error"] = "Kaytegori bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(updateDTO);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kategori bilgileri getirilirken hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Kategori Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _categoryService.UpdateAsync(model);
                TempData["Success"] = "Kategori başarılı bir şekilde güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

        // Kategori Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                TempData["Error"] = "Geçersiz kategori ID'si!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "Kategori bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                var detailDto = _mapper.Map<CategoryDetailDTO>(category);
                return View(detailDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Kategori Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.SoftDeleteAsync(id);
                TempData["Success"] = "Kategori başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Silinmiş Kategorileri Listele
        [HttpGet]
        public async Task<IActionResult> Trash(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            try
            {
                var deletedCategories = await _categoryService.GetAllDeletedAsync();

                if (!deletedCategories.Any())
                {
                    TempData["Info"] = "Henüz silinmiş bir ürün yok!";
                    return View(new ListViewModel<CategoryListDTO>
                    {
                        Items = new List<CategoryListDTO>(),
                        SearchQuery = search,
                        PaginationInfo = new PaginationInfo
                        {
                            Count = 0,
                            PageSize = pageSize,
                            CurrentPage = pageNumber
                        }
                    });
                }

                var filtered = deletedCategories.Filter(search, new Func<CategoryListDTO, string>[]
                {
                    x => x.Name,
                    x => x.Description ?? string.Empty
                });

                var paged = filtered.Paginate(pageNumber, pageSize).ToList();

                var viewModel = new ListViewModel<CategoryListDTO>
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
                TempData["Error"] = $"Silinmiş kategoriler yüklenemedi! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        // Kategori Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            try
            {
                await _categoryService.RestoreAsync(id);
                TempData["Success"] = "Kategori başarıyla geri yüklendi!";
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
                await _categoryService.DeleteAsync(id);
                TempData["Success"] = "Kategori kalıcı olarak silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kalıcı silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}