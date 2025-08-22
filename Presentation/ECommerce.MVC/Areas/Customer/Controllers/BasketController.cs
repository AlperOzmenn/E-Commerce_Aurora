using ECommerce.Application.DTOs.BasketsDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class BasketController : Controller
    {
        private readonly IBasketSessionService _basketSessionService;

        public BasketController(IBasketSessionService basketSessionService)
        {
            _basketSessionService = basketSessionService;
        }

        public IActionResult Index()
        {
            var basket = _basketSessionService.GetBasket();
            return View(basket);
        }

        [HttpPost]
        public IActionResult AddToBasket(string productId, string name, decimal price, string imageUrl, int quantity = 1)
        {
            var basket = _basketSessionService.GetBasket();

            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                basket.Items.Add(new BasketItemDTO
                {
                    ProductId = productId,
                    ProductName = name,
                    UnitPrice = price,
                    ImageUrl = imageUrl,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            _basketSessionService.SaveBasket(basket);
            //return RedirectToAction("Index");

            return Json(new { success = true, message = "Ürün sepete eklendi." });
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(string productId)
        {
            var basket = _basketSessionService.GetBasket();
            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity++;
                _basketSessionService.SaveBasket(basket);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(string productId)
        {
            var basket = _basketSessionService.GetBasket();
            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                _basketSessionService.SaveBasket(basket);
            }
            else if (item != null && item.Quantity <= 1)
            {
                basket.Items.Remove(item); // Adeti 0’a düşerse otomatik sil
                _basketSessionService.SaveBasket(basket);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(string productId)
        {
            var basket = _basketSessionService.GetBasket();
            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                basket.Items.Remove(item);
                _basketSessionService.SaveBasket(basket);
            }

            return RedirectToAction("Index");
        }
    }

}
