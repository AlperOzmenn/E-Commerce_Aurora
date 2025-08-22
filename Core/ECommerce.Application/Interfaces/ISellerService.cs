using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ISellerService : IGenericService<Seller, SellerCreateDTO, SellerUpdateDTO, SellerListDTO, SellerDTO>
    {
        Task<List<SellerListDTO>> GetPendingSellersAsync();
        Task<List<SellerListDTO>> GetApprovedSellersWithRoleAsync();
        Task<List<SellerListDTO>> GetAllDeletedWithAppUserAsync();
        Task<Guid> GetSellerIdByUserIdAsync(Guid userId);
    }
}