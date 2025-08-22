using AutoMapper;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Persistence.Concrates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public CommentController(ICommentService commentService, IMapper mapper, IProductService productService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _productService = productService;
        }

        // ✅ Giriş yapan kullanıcının Guid tipindeki ID'sini al
        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdStr, out Guid userId))
                return userId;

            throw new UnauthorizedAccessException("Kullanıcı ID'si alınamadı.");
        }

        // ✅ Tüm yorumlar listesi (isteğe bağlı: sadece kendi yorumların gösterilebilir)
        public async Task<IActionResult> Index()
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments);
        }

        // ✅ Yorum detayı (herkes görebilir)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(Guid id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);

                if (comment == null)
                {
                    TempData["Error"] = "Yorum bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(comment);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Yorum getirme sırasında hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AddComment(CommentCreateDTO dto)
        //{
        //    dto.AppUserId = GetCurrentUserId();
        //    await _commentService.AddAsync(dto);
        //    return RedirectToAction("Detail", "Product", new { id = dto.ProductId });
        //}

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreateDTO model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // İçeriğe seller etiketi ekle
            model = model with { Content = $"[Seller]: {model.Content}" };


            var result = await _commentService.AddCommentAsync(model, Guid.Parse(userId));

            return Ok(result); // DTO döner
        }






        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            var product = await _productService.GetByIdAsync(comment.ProductId);
            var currentUserId = GetCurrentUserId();

            if (product.SellerId != currentUserId)
                return Forbid();

            return View(comment);
        }
    }
}
