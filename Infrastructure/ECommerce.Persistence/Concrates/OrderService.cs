using AutoMapper;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Persistence.Concrates
{
    public class OrderService : GenericService<Order, OrderCreateDTO, OrderUpdateDTO, OrderListDTO, OrderDTO>, IOrderService
    {
        private readonly IRepository<Order> _repository;

        public OrderService(IRepository<Order> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _repository.FindConditionAsync(x => x.AppUserId == userId, isTrack: false);
        }
        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId)
        {
            return await _repository.FindConditionAsync(x => x.AppUserId == sellerId, isTrack: false);
        }
    }
}