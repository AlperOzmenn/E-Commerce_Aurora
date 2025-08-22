using AutoMapper;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdStr, out Guid userId))
                return userId;

            throw new UnauthorizedAccessException("Kullanıcı ID'si alınamadı.");
        }

        // Sipariş Listesi
        public async Task<IActionResult> Index()
        {
            try
            {
                Guid sellerId = GetCurrentUserId();
                var orderEntities = await _orderService.GetOrdersBySellerIdAsync(sellerId);

                // AutoMapper ile Order -> OrderListDTO çevir
                var orderDTOs = _mapper.Map<List<OrderListDTO>>(orderEntities);

                return View(orderDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Oturum açmanız gerekiyor.";
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        // Sipariş Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Geçersiz sipariş ID!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(order); // order tipi OrderDetailDTO
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş detayları alınırken hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Güncelleme Sayfası (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Geçersiz sipariş ID!";
                return RedirectToAction(nameof(Index));
            }

            var order = await _orderService.GetByIdAsync(id); // DTO veya Entity geliyor olabilir
            if (order == null)
            {
                TempData["Error"] = "Sipariş bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            // DTO'ya map'le
            var updateDto = _mapper.Map<OrderUpdateDTO>(order);
            return View(updateDto);
        }

        // Güncelleme İşlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Formda hatalar var.";
                return View(dto);
            }

            try
            {
                await _orderService.UpdateAsync(dto);
                TempData["Success"] = "Sipariş başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme sırasında hata oluştu: {ex.Message}";
                return View(dto);
            }
        }



    }
}