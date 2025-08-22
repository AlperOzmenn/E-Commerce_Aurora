using ECommerce.MVC.ViewModels.PaymentVMs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pay(PaymentVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Bu noktada sanal pos/ödeme API'sine istek yapılmalı
            // Test amaçlı sahte onay verelim
            bool isPaymentSuccessful = FakePaymentProcess(model);

            if (isPaymentSuccessful)
            {
                // siparişi tamamlama, veritabanına kaydetme vs.
                return RedirectToAction("Success");
            }

            ModelState.AddModelError("", "Ödeme başarısız oldu.");
            return View(model);
        }

        private bool FakePaymentProcess(PaymentVM model)
        {
            // Gerçek dünyada bu kısımda ödeme API'sine istek yapılır
            return true;
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
