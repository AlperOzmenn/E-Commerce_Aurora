using ECommerce.Application.Interfaces;
using ECommerce.Application.ViewModels;
using ECommerce.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

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
    }
}
