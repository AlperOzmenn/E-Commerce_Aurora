using AutoMapper;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Concrates;
using ECommerce.Persistence.Contexts;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public CommentController(ICommentService commentService, IMapper mapper,IProductService productService,AppDbContext dbContext)
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
            try
            {
                Guid currentUserId = GetCurrentUserId();
                var comments = await _commentService.GetCommentsByUserIdAsync(currentUserId);
                return View(comments);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Oturum açmanız gerekiyor.";
                return RedirectToAction("Login", "Account");
            }
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

        //    var area = ControllerContext.RouteData.Values["area"]?.ToString();

        //    return RedirectToAction("Detail", "Product", new { area = Unauthorized(), id = dto.ProductId });

        //}

        //[HttpPost]
        //[AllowAnonymous] // Bu önemli! Yetkisiz kullanıcı buraya erişebilsin ki yönlendirme yapılabilsin
        //public async Task<IActionResult> AddComment(CommentCreateDTO dto)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Register", "Account");
        //    }

        //    dto.AppUserId = GetCurrentUserId();
        //    await _commentService.AddAsync(dto);

        //    return RedirectToAction("Detail", "Product", new { area = Unauthorized(), id = dto.ProductId });
        //}

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreateDTO model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
