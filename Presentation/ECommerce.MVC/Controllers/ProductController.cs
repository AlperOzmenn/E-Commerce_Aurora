using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.ViewModels;
using ECommerce.Persistence.Concrates;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService,ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

        // Ürün Listesi
        public async Task<IActionResult> Index()
        {
            var dtoProduct = await _productService.GetAllWithCategoryAsync();
            return View(dtoProduct);
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

            return await ExecuteSafeAsync(
                async () =>
                {
                    await _productService.AddAsync(model);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Ürün başarılı bir şekilde eklendi!",
                errorMessage: "Ürün eklenirken bir hata oluştu!"
                    );
        }

        // Ürün Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(
                 async () =>
                 {
                     var productDto = await _productService.GetByIdAsync(id);

                     if (productDto == null)
                         return NotFound();

                     var updateDTO = new ProductUpdateDTO
                     {
                         Id = productDto.Id,
                         Name = productDto.Name,
                         Description = productDto.Description,
                         Price = productDto.Price,
                         Stock = productDto.Stock,
                     };

                     return View(updateDTO);
                 },
                 errorMessage: "Ürün bilgileri getirilirken hata oluştu!"
                 );
            
        }

        // Ürün Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(
                async () =>
                {
                    await _productService.UpdateAsync(model);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Ürün başarıyla güncellendi!",
                errorMessage: "Ürün güncelleme işlemi sırasında bir hata oluştu!"
            );
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
            return await ExecuteSafeAsync(
               async () =>
        {
               await _productService.SoftDeleteAsync(id);
               return RedirectToAction(nameof(Index));
        },
             successMessage: "Ürün başarıyla silindi!",
             errorMessage: "Silme işlemi sırasında hata oluştu!"
               );
        }

        // Silinmiş Ürünleri Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    var deletedProducts = await _productService.GetAllDeletedAsync();

                    if (!deletedProducts.Any())
                        TempData["Info"] = "Henüz silinmiş bir ürün yok!";

                    return View(deletedProducts);
                },
                errorMessage: "Silinmiş ürünler yüklenemedi!"
            );
        }

        // Ürün Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(
                async () =>
                {
                    await _productService.RestoreAsync(id);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Ürün başarıyla geri yüklendi!",
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
                    await _productService.DeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                },
                successMessage: "Ürün kalıcı olarak silindi!",
                errorMessage: "Kalıcı silme sırasında hata oluştu!"
            );
        }
    }
}
