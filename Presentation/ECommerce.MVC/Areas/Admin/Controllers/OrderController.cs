using ECommerce.Application.Common;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Sipariş Listesi
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            var orders = await _orderService.GetAllAsync();

            var filtered = orders.Filter(search, new Func<OrderListDTO, string>[]
            {
                x => x.OrderNumber,
                x => x.CustomerName,
                x => x.Status.ToString()
            });

            var paged = filtered.Paginate(pageNumber, pageSize).ToList();

            var viewModel = new ListViewModel<OrderListDTO>
            {
                Items = paged,
                SearchQuery = search,
                PaginationInfo = new PaginationInfo
                {
                    Count = filtered.Count(),
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                }
            };

            return View(viewModel);
        }

        // Sipariş Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var orderDto = await _orderService.GetByIdAsync(id);

                OrderUpdateDTO updateDTO = new OrderUpdateDTO
                {
                    Id = orderDto.Id,
                    Status = orderDto.Status,
                };

                if (orderDto == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(updateDTO);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş bilgileri getirilirken hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Sipariş Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(OrderUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _orderService.UpdateAsync(model);
                TempData["Success"] = "Sipariş başarılı bir şekilde güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
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
