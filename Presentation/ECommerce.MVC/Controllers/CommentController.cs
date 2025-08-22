using AutoMapper;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Controllers
{
    [Authorize]
    public class CommentController : BaseController
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

            return await ExecuteSafeAsync(async () =>
            {
                model.AppUserId = GetCurrentUserId();
                await _commentService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Yorum başarılı bir şekilde eklendi!", errorMessage: "Yorum ekleme sırasında hata oluştu!");
        }

        // ✅ Yorum güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null)
                {
                    TempData["Error"] = "Yorum bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                if (comment.AppUserId != GetCurrentUserId())
                {
                    TempData["Error"] = "Bu yorumu düzenleme yetkin yok!";
                    return RedirectToAction(nameof(Index));
                }

                var commentDto = _mapper.Map<CommentUpdateDTO>(comment);
                return View(commentDto);
            }, errorMessage: "Yorum bilgileri getirilirken hata oluştu!");
        }

        // ✅ Yorum güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CommentUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
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

                await _commentService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Yorum başarılı bir şekilde güncellendi!", errorMessage: "Yorum güncelleme sırasında hata oluştu!");
        }

        // ✅ Yorum detayı (herkes görebilir)
        [HttpGet]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null)
                {
                    TempData["Error"] = "Yorum bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(comment);
            }, errorMessage: "Yorum getirme sırasında hata oluştu!");
        }

        // ✅ Soft delete (sadece kendi yorumu)
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null || comment.AppUserId != GetCurrentUserId())
                {
                    TempData["Error"] = "Bu yorumu silme yetkin yok!";
                    return RedirectToAction(nameof(Index));
                }

                await _commentService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Silme işlemi başarılı!", errorMessage: "Silme sırasında hata oluştu!");
        }

        // ✅ Silinmiş yorumlar listesi (kendi yorumları)
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var deletedComments = await _commentService.GetAllDeletedAsync();
                var currentUserId = GetCurrentUserId();

                var myDeletedComments = deletedComments.Where(c => c.AppUserId == currentUserId).ToList();

                if (!myDeletedComments.Any())
                    TempData["Info"] = "Henüz silinmiş bir yorumun yok.";

                return View(myDeletedComments);
            }, errorMessage: "Silinmiş yorumlar yüklenemedi!");
        }

        // ✅ Restore (sadece kendi yorumu)
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null || comment.AppUserId != GetCurrentUserId())
                {
                    TempData["Error"] = "Bu yorumu geri yükleme yetkin yok!";
                    return RedirectToAction(nameof(Index));
                }

                await _commentService.RestoreAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Yorum geri yüklendi!", errorMessage: "Geri yükleme sırasında hata oluştu!");
        }

        // ✅ Hard delete (sadece kendi yorumu)
        public async Task<IActionResult> HardDelete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null || comment.AppUserId != GetCurrentUserId())
                {
                    TempData["Error"] = "Bu yorumu kalıcı silme yetkin yok!";
                    return RedirectToAction(nameof(Index));
                }

                await _commentService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Yorum kalıcı olarak silindi!", errorMessage: "Kalıcı silme sırasında hata oluştu!");
        }
    }
}
