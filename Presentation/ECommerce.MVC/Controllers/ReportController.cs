using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO> _reportService;

        public ReportController(UserManager<AppUser> userManager, IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO> reportService)
        {
            _userManager = userManager;
            _reportService = reportService;
        }

        // Yeni oluşturma formu
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni rapor oluşturma
        [HttpPost]
        public async Task<IActionResult> Create(ReportCreateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login", "Account");
            }

            

            await _reportService.AddAsync(dto);
            TempData["Success"] = "Rapor başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

    }
}
