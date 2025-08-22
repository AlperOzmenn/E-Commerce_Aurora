using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public SellerController(ISellerService sellerService, IAppUserService appUserService, IMapper mapper)
        {
            _sellerService = sellerService;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        // Satıcı onayı bekleyen kullanıcıları listele
        [HttpGet]
        public async Task<IActionResult> PendingSeller(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            // Onay bekleyen satıcıları al
            var sellers = await _sellerService.GetPendingSellersAsync();

            //Arama
            var filteredSellers = sellers.Filter(search, new Func<SellerListDTO, string>[]
            {
                x => x.CompanyName,
                x => x.Email,
                x => x.ContactTitle,
                x => x.ContactName,
                x => x.Name,
                x => x.Surname
            });

            //Sayfalama
            var pagedSellers = filteredSellers.Paginate(pageNumber, pageSize).ToList();

            // Generic ViewModel
            var viewModel = new ListViewModel<SellerListDTO>
            {
                Items = pagedSellers,
                SearchQuery = search,
                PaginationInfo = new PaginationInfo
                {
                    Count = filteredSellers.Count(),
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                }
            };

            return View(viewModel);
        }

        // Satıcıyı onaylama
        [HttpPost]
        public async Task<IActionResult> ApprovedSeller(Guid id)
        {
            var sellerEntity = await _sellerService.GetEntityByIdAsync(id);

            if (sellerEntity is null)
            {
                TempData["Error"] = "Satıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var now = DateTime.Now; // İki tane tarih güncellemesi yapacağımız için burda tanımlandı.

                // Seller güncelleme
                sellerEntity.Approved = true;
                sellerEntity.UpdatedDate = now;
                await _sellerService.UpdateAsync(_mapper.Map<SellerUpdateDTO>(sellerEntity));

                // AppUser güncelleme
                var appUser = await _appUserService.GetEntityByIdAsync(sellerEntity.AppUserId);
                if (appUser is not null)
                {
                    appUser.UpdatedDate = now;
                    await _appUserService.UpdateAsync(_mapper.Map<AppUserUpdateDTO>(appUser));
                }

                // Rol ekleme
                await _appUserService.AddUserToRoleAsync(sellerEntity.AppUserId, "Seller");

                TempData["Success"] = "Satıcı onaylandı!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Satıcı onaylanırken bir hata oluştu! Hata mesajı: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Satıcı rolü reddetme
        [HttpPost]
        public async Task<IActionResult> RejectSeller(Guid id)
        {
            var seller = await _sellerService.GetEntityByIdAsync(id);

            if (seller is null)
            {
                TempData["Error"] = "Satıcı bulunamadı!";
                return RedirectToAction(nameof (Index));
            }

            try
            {
                //seller.UpdatedDate = DateTime.Now;
                await _sellerService.DeleteAsync(id);

                TempData["Success"] = "Reddetme işlemi başatıyla gerçekleşti!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Reddetme işlemi sırasında bir hata oluştu. Hata mesajı: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        //Satıcı Listele
        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            int pageSize = 20;

            var approvedSellers = await _sellerService.GetApprovedSellersWithRoleAsync();

            // Arama
            var filtered = approvedSellers.Filter(search, new Func<SellerListDTO, string>[]
            {
                x => x.CompanyName,
                x => x.Email,
                x => x.ContactTitle,
                x => x.ContactName,
                x => x.Name,
                x => x.Surname
            });

            // Sayfalama
            var paged = filtered.Paginate(pageNumber, pageSize).ToList();

            // Generic ViewModel
            var viewModel = new ListViewModel<SellerListDTO>
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

        // Satıcı Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var sellerDTO = await _sellerService.GetByIdAsync(id);

                SellerUpdateDTO updateDTO = new SellerUpdateDTO
                {
                    CompanyName = sellerDTO.CompanyName,
                    ContactName = sellerDTO.ContactName,
                    ContactTitle = sellerDTO.ContactTitle
                };

                if (sellerDTO == null)
                {
                    TempData["Error"] = "Satıcı bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                return View(updateDTO);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Satıcı bilgileri getirilirken hata oluştu! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));

            }
        }

        // Satıcı Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(SellerUpdateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _sellerService.UpdateAsync(model);
                TempData["Success"] = "Satıcı başarılı bir şekilde güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme sırasında hata oluştu! Hata: {ex.Message}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Geçersiz satıcı ID'si!";
                return RedirectToAction(nameof(Index));
            }

            var seller = await _sellerService.GetByIdAsync(id);

            if (seller == null)
            {
                TempData["Error"] = "Satıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            return View(seller);
        }

        // Satıcı Soft Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _sellerService.SoftDeleteAsync(id);
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // Silinmiş Satıcıları Listele
        [HttpGet]
        public async Task<IActionResult> Trash(string search, int pageNumber = 1)
        {
            try
            {
                int pageSize = 20;

                var deletedSellers = await _sellerService.GetAllDeletedWithAppUserAsync();

                if (!deletedSellers.Any())
                {
                    TempData["Info"] = "Henüz silinmiş bir ürün yok!";
                    return View(new ListViewModel<SellerListDTO>
                    {
                        Items = new List<SellerListDTO>(),
                        SearchQuery = search,
                        PaginationInfo = new PaginationInfo
                        {
                            Count = 0,
                            PageSize = pageSize,
                            CurrentPage = pageNumber
                        }
                    });
                }

                var filtered = deletedSellers.Filter(search, new Func<SellerListDTO, string>[]
                {
                     x => x.CompanyName,
                     x => x.Email,
                     x => x.ContactTitle,
                     x => x.ContactName,
                     x => x.Name,
                     x => x.Surname
                });

                var paged = filtered.Paginate(pageNumber, pageSize).ToList();

                var viewModel = new ListViewModel<SellerListDTO>
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
            catch (Exception ex)
            {
                TempData["Error"] = $"Silinmiş satıcılar yüklenemedi! Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Satıcı Restore Etme
        [HttpGet]
        public async Task<IActionResult> Restore(Guid id)
        {
            try
            {
                await _sellerService.RestoreAsync(id);
                TempData["Success"] = "Satıcı başarıyla geri yüklendi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Geri yükleme işlemi sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Trash));
        }

        // Kalıcı Silme (Hard Delete)
        [HttpGet]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            try
            {
                await _sellerService.DeleteAsync(id);
                TempData["Success"] = "Satıcı kalıcı olarak silindi!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kalıcı silme sırasında hata oluştu! Hata: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}