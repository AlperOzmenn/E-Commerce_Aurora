using AutoMapper;
using ECommerce.Application.DTOs.AddressDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class AddressController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepository<Address> _addressRepo;
        private readonly IRepository<AddressAppUser> _addressAppUserRepo;
        private readonly IMapper _mapper;

        public AddressController( UserManager<AppUser> userManager, IRepository<Address> addressRepo,
            IRepository<AddressAppUser> addressAppUserRepo, IMapper mapper)
        {
            _userManager = userManager;
            _addressRepo = addressRepo;
            _addressAppUserRepo = addressAppUserRepo;
            _mapper = mapper;
        }

        // 📌 Listele
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var addressLinks = await _addressAppUserRepo.GetFilteredListAsync(
                select: x => x,
                where: x => x.AppUserId == user.Id,
                join: q => q.Include(x => x.Address)
            );

            var listDtos = addressLinks.Select(x => _mapper.Map<AddressListDTO>(x.Address)).ToList();

            return View(listDtos);
        }

        // 📌 Detay
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if (address == null) return NotFound();

            var dto = _mapper.Map<AddressDetailDTO>(address);
            return View(dto);
        }

        // 📌 Ekle GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 📌 Ekle POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var address = _mapper.Map<Address>(dto);
            _addressRepo.Add(address);

            var user = await _userManager.GetUserAsync(User);

            _addressAppUserRepo.Add(new AddressAppUser
            {
                AppUserId = user.Id,
                AddressId = address.Id,
                IsDefault = false
            });

            return RedirectToAction(nameof(Index));
        }

        // 📌 Güncelle GET
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if (address == null) return NotFound();

            var dto = _mapper.Map<AddressUpdateDTO>(address);
            return View(dto);
        }

        // 📌 Güncelle POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddressUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var address = _mapper.Map<Address>(dto);
            address.Update();

            _addressRepo.Update(address);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Sil
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if (address == null) return NotFound();

            address.SoftDelete();
            _addressRepo.Update(address);

            return RedirectToAction(nameof(Index));
        }
    }
}
