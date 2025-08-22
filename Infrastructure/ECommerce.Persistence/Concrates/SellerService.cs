using AutoMapper;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Concrates
{
    public class SellerService : GenericService<Seller, SellerCreateDTO, SellerUpdateDTO, SellerListDTO, SellerDTO>, ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Seller> _repository;

        public SellerService(IRepository<Seller> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        // Satıcı onayı bekleyen kullanıcıları listeler.
        public async Task<List<SellerListDTO>> GetPendingSellersAsync()
        {
            var sellers = await _unitOfWork
                .GetRepository<Seller>()
                .GetFilteredListAsync(
                    select: s => s,
                    where: s => !s.Approved && s.AppUser != null && !s.AppUser.IsDeleted,
                    join: s => s.Include(s => s.AppUser)
                );

            return _mapper.Map<List<SellerListDTO>>(sellers);
        }

        // Onaylanmış sellerları getirir.
        public async Task<List<SellerListDTO>> GetApprovedSellersWithRoleAsync()
        {
            var sellers = await _unitOfWork
                .GetRepository<Seller>()
                .GetFilteredListAsync<Seller>(
                    select: s => s,
                    where: s => s.Approved && s.AppUser != null && !s.AppUser.IsDeleted,
                    join: s => s.Include(s => s.AppUser)
                                .ThenInclude(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
            );

            //var filtered = sellers
            //    .Where(s => s.AppUser.UserRoles.Any(ur => ur.Role.Name == "Seller"))
            //    .ToList();

           // return _mapper.Map<List<SellerListDTO>>(filtered);
            return _mapper.Map<List<SellerListDTO>>(sellers);
        }

        // Silinen sellerları kullanıcı adlarıyla birlikte getirir.
        public async Task<List<SellerListDTO>> GetAllDeletedWithAppUserAsync()
        {
            var deletedSellers = await _unitOfWork
                .GetRepository<Seller>()
                .GetFilteredListAsync(
                    select: s => s,
                    where: s => s.IsDeleted,
                    join: q => q
                .IgnoreQueryFilters()
                .Include(s => s.AppUser)
                );
            return _mapper.Map<List<SellerListDTO>>(deletedSellers);
        }

        public async Task<Guid> GetSellerIdByUserIdAsync(Guid userId)
        {
            var seller = await _unitOfWork
                .GetRepository<Seller>()
                .FirstOrDefaultAsync(s => s.AppUserId == userId, isTrack: false);

            if (seller == null)
                throw new Exception("Seller bulunamadı");

            return seller.Id;
        }

        //public async Task<SellerListDTO> GetSellerWithUserByIdAsync(Guid id)
        //{
        //    var seller = await _repository  
        //        .get
        //}
    }
}