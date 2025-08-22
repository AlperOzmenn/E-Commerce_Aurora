using AutoMapper;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
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

        // ✅ Yorum ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Yorum ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 🆕 Kullanıcı ID’sini modele ata
                model.AppUserId = GetCurrentUserId();

                await _commentService.AddAsync(model);
                TempData["Success"] = "Yorum başarılı bir şekilde eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Yorum ekleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

        // ✅ Yorum güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null)
            {
                TempData["Error"] = "Yorum bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            // 🆕 Kullanıcı sadece kendi yorumunu düzenleyebilir
            if (comment.AppUserId != GetCurrentUserId())
            {
                TempData["Error"] = "Bu yorumu düzenleme yetkin yok!";
                return RedirectToAction(nameof(Index));
            }

            var commentDto = _mapper.Map<CommentUpdateDTO>(comment);
            return View(commentDto);
        }

        // ✅ Yorum güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CommentUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var comment = await _commentService.GetByIdAsync(model.Id);
            if (comment == null)
            {
                TempData["Error"] = "Yorum bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            if (comment.AppUserId != GetCurrentUserId())
            {
                TempData["Error"] = "Bu yorumu güncelleme yetkin yok!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _commentService.UpdateAsync(model);
                TempData["Success"] = "Yorum başarılı bir şekilde güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Yorum güncelleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
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

        // ✅ Soft delete (sadece kendi yorumu)
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null || comment.AppUserId != GetCurrentUserId())
            {
                TempData["Error"] = "Bu yorumu silme yetkin yok!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _commentService.SoftDeleteAsync(id);
                TempData["Success"] = "Silme işlemi başarılı!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Silinmiş yorumlar listesi (kendi yorumları)
        public async Task<IActionResult> Trash()
        {
            try
            {
                var deletedComments = await _commentService.GetAllDeletedAsync();
                var currentUserId = GetCurrentUserId();

                // 🆕 Sadece kendi silinmiş yorumlarını filtrele (bu kontrolü service'de de yapabilirsin)
                var myDeletedComments = deletedComments.Where(c => c.AppUserId == currentUserId).ToList();

                if (!myDeletedComments.Any())
                    TempData["Info"] = "Henüz silinmiş bir yorumun yok.";

                return View(myDeletedComments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silinmiş yorumlar yüklenemedi! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // ✅ Restore (sadece kendi yorumu)
        public async Task<IActionResult> Restore(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null || comment.AppUserId != GetCurrentUserId())
            {
                TempData["Error"] = "Bu yorumu geri yükleme yetkin yok!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _commentService.RestoreAsync(id);
                TempData["Success"] = "Yorum geri yüklendi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Geri yükleme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Hard delete (sadece kendi yorumu)
        public async Task<IActionResult> HardDelete(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null || comment.AppUserId != GetCurrentUserId())
            {
                TempData["Error"] = "Bu yorumu kalıcı silme yetkin yok!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _commentService.DeleteAsync(id);
                TempData["Success"] = "Yorum kalıcı olarak silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kalıcı silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
