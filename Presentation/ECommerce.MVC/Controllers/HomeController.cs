using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        // Index action, servisden ProductListDTO tipinde ürünler alır, View'a gönderir
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllWithCategoryAsync();
            return View(products);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}