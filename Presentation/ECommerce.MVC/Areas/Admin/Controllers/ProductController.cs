using ECommerce.Application.Common;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Ürün Listesi
        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            var products = await _productService.GetAllAsync();

            //Aranan metinleri filtreleme
            var filtered = products.Filter(search, new Func<ProductListDTO, string>[]
            {
                x => x.Name,
                x => x.Description,
                x => x.Stock.ToString(),
            });

            var paged = filtered.Paginate(pageNumber, pageSize).ToList();

            var viewModel = new ListViewModel<ProductListDTO>
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


        // Ürün Ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Ürün Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _productService.AddAsync(model);
                TempData["Success"] = "Ürün başarılı bir şekilde eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün ekleme işlemi sırasında bir hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

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

        // Ürün Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                TempData["Error"] = "Geçersiz ürün ID'si!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    TempData["Error"] = "Ürün bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Trash(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            try
            {
                var deletedProducts = await _productService.GetAllDeletedAsync();

                if (!deletedProducts.Any())
                {
                    TempData["Info"] = "Henüz silinmiş bir ürün yok!";
                    return View(new ListViewModel<ProductListDTO>
                    {
                        Items = new List<ProductListDTO>(),
                        SearchQuery = search,
                        PaginationInfo = new PaginationInfo
                        {
                            Count = 0,
                            PageSize = pageSize,
                            CurrentPage = pageNumber
                        }
                    });
                }

                var filtered = deletedProducts.Filter(search, new Func<ProductListDTO, string>[]
                {
                    x => x.Name,
                    x => x.Description,
                    x => x.Stock.ToString(),
                });

                var paged = filtered.Paginate(pageNumber, pageSize).ToList();

                var viewModel = new ListViewModel<ProductListDTO>
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
