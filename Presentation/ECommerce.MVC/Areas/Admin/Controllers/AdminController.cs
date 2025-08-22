using ECommerce.Application.Interfaces;
using ECommerce.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IAppUserService _userService;

        public AdminController(IOrderService orderService,IProductService productService,ISellerService sellerService,IAppUserService appUserService)
        {
            _orderService = orderService;
            _productService = productService;
            _sellerService = sellerService;
            _userService = appUserService;
        }

        public async Task<IActionResult> Index()
        {
            var allProducts = await _productService.GetAllAsync();
            var orders = await _orderService.GetAllAsync();
            var pendingSellers = await _sellerService.GetPendingSellersAsync();
            var allUsers = await _userService.GetAllAsync();

            var todayRegisteredUsers = allUsers
                .Where(u => u.CreatedDate.Date == DateTime.Today)
                .ToList();
            var newProducts = allProducts
                .Where(p => p.CreatedDate >= DateTime.Now.AddDays(-7))
                .ToList();
            var model = new DashboardViewModel
            {
                TotalOrders = orders.Count(),
                NewProducts = newProducts.Count(),
                SellerApplications = pendingSellers.Count(),
                TodayVisitors = todayRegisteredUsers.Count()
            };

            return View(model);
        }
    }
}
