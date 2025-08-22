using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService : IGenericService<Order, OrderCreateDTO, OrderUpdateDTO, OrderListDTO, OrderDTO>
    {
        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId);
    }
}
