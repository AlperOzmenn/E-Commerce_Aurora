using ECommerce.Application.DTOs.PaymentDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Enums.PaymentEnums;
using ECommerce.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Kullanıcının kendi ödemelerini görmesi
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payments = await _paymentService.GetAllByUserIdAsync(Guid.Parse(userId));
            return View(payments);
        }

        [HttpGet]
        public IActionResult TestPayment()
        {
            var testDto = new PaymentCreateDTO
            {
                Amount = 799.00m,
                PaymentDate = DateTime.Now,
                Status = PaymentStatus.Pending,
                TransactionId = "TRX123456789",
                Method = PaymentMethod.CreditCard,
                CardName = "Eda Yazılım",
                CardNumber = "4111111111111111",
                ExpirationMonth = "12",
                ExpirationYear = "2027",
                CVV = "123",
                OrderDetailId = Guid.Parse("DENEYSEL-BİR-GUID-YERLEŞTİR"), // Buraya gerçek siparişin ID’sini yaz
                AppUserId = Guid.Parse("b732ca5a-243e-45ff-78a0-08ddcd26a790")// Bu da login olan kullanıcının ID’si olacak
            };

            return View("Create", testDto);
        }


        // Ödeme oluşturma formu
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.AppUserId = Guid.Parse(userId);
            dto.PaymentDate = DateTime.Now;
            dto.Status = Domain.Enums.PaymentEnums.PaymentStatus.Pending;

            await _paymentService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

   


        // Ödeme Detayları
        public async Task<IActionResult> Detail(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var payment = await _paymentService.GetByIdAsync(id);

            if (payment == null || payment.AppUserId != userId)
                return Unauthorized();

            return View(payment);
        }

        // İsteğe bağlı: Ödeme silme (sadece kendi ödemesi için)
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var payment = await _paymentService.GetByIdAsync(id);

            if (payment == null || payment.AppUserId != userId)
                return Unauthorized();

            await _paymentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}