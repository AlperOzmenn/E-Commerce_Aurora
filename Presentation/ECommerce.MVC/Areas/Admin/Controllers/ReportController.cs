using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO> _reportService;

        public ReportController(IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO> reportService)
        {
            _reportService = reportService;
        }

        // Listeleme
        public async Task<IActionResult> Index()
        {
            var reports = await _reportService.GetAllAsync();
            return View(reports);
        }

        // Detay
        public async Task<IActionResult> Detail(Guid id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null)
            {
                TempData["Error"] = "Rapor bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }


        //// Güncelleme formu
        //[HttpGet]
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var report = await _reportService.GetByIdAsync(id);
        //    if (report == null) return NotFound();

        //    var updateDto = new ReportUpdateDTO
        //    {
        //        Id = report.Id,
        //        Subject = report.Subject,
        //        Message = report.Message
        //    };

        //    return View(updateDto);
        //}

        //// Güncelleme işlemi
        //[HttpPost]
        //public async Task<IActionResult> Edit(ReportUpdateDTO dto)
        //{
        //    if (!ModelState.IsValid) return View(dto);

        //    await _reportService.UpdateAsync(dto);
        //    TempData["Success"] = "Rapor başarıyla güncellendi.";
        //    return RedirectToAction(nameof(Index));
        //}

        // Silme (Soft Delete)
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reportService.SoftDeleteAsync(id);
            TempData["Warning"] = "Rapor silindi.";
            return RedirectToAction(nameof(Index));
        }

        // Kalıcı Silme
        public async Task<IActionResult> HardDelete(Guid id)
        {
            await _reportService.DeleteAsync(id);
            TempData["Danger"] = "Rapor kalıcı olarak silindi.";
            return RedirectToAction(nameof(Index));
        }

        // Geri Yükleme
        public async Task<IActionResult> Restore(Guid id)
        {
            await _reportService.RestoreAsync(id);
            TempData["Success"] = "Rapor geri yüklendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
