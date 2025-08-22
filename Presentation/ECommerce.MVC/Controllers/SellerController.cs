using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class SellerController : BaseController
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public async Task<IActionResult> Index()
        {
            var sellers = await _sellerService.GetAllAsync();
            return View(sellers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SellerCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
                await _sellerService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Satıcı başarılı bir şekilde eklendi.", errorMessage: "Satıcı ekleme işlemi sırasında bir hata oluştu!");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var sellerDTO = await _sellerService.GetByIdAsync(id);
                if (sellerDTO == null)
                {
                    TempData["Error"] = "Satıcı bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }
                return View(sellerDTO);
            }, errorMessage: "Satıcı bilgileri getirilirken hata oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
                await _sellerService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Satıcı başarılı bir şekilde güncellendi!", errorMessage: "Güncelleme sırasında hata oluştu!");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Geçersiz kullanıcı ID'si!";
                return RedirectToAction(nameof(Index));
            }

            return await ExecuteSafeAsync(async () =>
            {
                var seller = await _sellerService.GetByIdAsync(id);
                if (seller == null)
                {
                    TempData["Error"] = "Satıcı bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }
                return View(seller);
            }, errorMessage: "Detay getirme sırasında hata oluştu!");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _sellerService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Kullanıcı başarıyla silindi!", errorMessage: "Silme işlemi sırasında hata oluştu!");
        }

        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var deletedSellers = await _sellerService.GetAllDeletedAsync();
                if (!deletedSellers.Any())
                    TempData["Info"] = "Henüz silinmiş bir satıcı yok!";

                return View(deletedSellers);
            }, errorMessage: "Silinmiş satıcılar yüklenemedi!");
        }

        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _sellerService.RestoreAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Satıcı başarıyla geri yüklendi!", errorMessage: "Geri yükleme işlemi sırasında hata oluştu!");
        }

        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _sellerService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Satıcı kalıcı olarak silindi!", errorMessage: "Kalıcı silme sırasında hata oluştu!");
        }
    }
}
