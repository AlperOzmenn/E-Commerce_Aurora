using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<IActionResult> ExecuteSafeAsync(
        Func<Task<IActionResult>> action,
        string successMessage = null,
        string errorMessage = null)
        {
            try
            {
                var result = await action();

                if (!string.IsNullOrEmpty(successMessage))
                    TempData["Success"] = successMessage;

                return result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = errorMessage ?? $"İşlem sırasında hata oluştu! Hata: {ex.Message}";
                return View(); // Geri dönülen view gerektiğinde override edilir
            }
        }
    }
}
