using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.MVC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Sipariş Listesi
        public async Task<IActionResult> Index()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var orders = await _orderService.GetAllAsync();
                return View(orders);
            }, errorMessage: "Siparişler yüklenirken hata oluştu!");
        }

        // Sipariş Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (id == Guid.Empty)
                {
                    TempData["Error"] = "Geçersiz sipariş ID'si!";
                    return RedirectToAction(nameof(Index));
                }

                var orderDto = await _orderService.GetByIdAsync(id);
                if (orderDto == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                var updateDTO = new OrderUpdateDTO
                {
                    Id = orderDto.Id,
                    Status = orderDto.Status,
                };

                return View(updateDTO);
            }, errorMessage: "Sipariş bilgileri getirilirken hata oluştu!");
        }

        // Sipariş Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(OrderUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return await ExecuteSafeAsync(async () =>
            {
                await _orderService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Sipariş başarılı bir şekilde güncellendi!", errorMessage: "Güncelleme sırasında hata oluştu!");
        }

        // Sipariş Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (id == Guid.Empty)
                {
                    TempData["Error"] = "Geçersiz sipariş ID'si!";
                    return RedirectToAction(nameof(Index));
                }

                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(order);
            }, errorMessage: "Detay getirme sırasında hata oluştu!");
        }

        // Sipariş Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _orderService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Sipariş başarıyla silindi!", errorMessage: "Silme işlemi sırasında hata oluştu!");
        }

        // Silinmiş Siparişları Listele
        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var deletedOrders = await _orderService.GetAllDeletedAsync();
                if (!deletedOrders.Any())
                    TempData["Info"] = "Henüz silinmiş bir sipariş yok!";

                return View(deletedOrders);
            }, errorMessage: "Silinmiş siparişler yüklenemedi!");
        }

        // Sipariş Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _orderService.RestoreAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Sipariş başarıyla geri yüklendi!", errorMessage: "Geri yükleme işlemi sırasında hata oluştu!");
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                await _orderService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }, successMessage: "Sipariş kalıcı olarak silindi!", errorMessage: "Kalıcı silme sırasında hata oluştu!");
        }
    }
}
