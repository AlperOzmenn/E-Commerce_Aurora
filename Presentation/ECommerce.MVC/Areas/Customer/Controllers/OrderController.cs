
using System.Security.Claims;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Sipariş Listesi (sadece kullanıcının siparişleri)
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return Unauthorized(); // giriş yapılmamışsa veya geçersizse

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return View(orders);
        }

        // Sipariş Detayı
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                TempData["Error"] = "Geçersiz kullanıcı ID'si!";
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

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Detay getirme sırasında hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        

        

    
      
    }
}
