using AutoMapper;
using ECommerce.Application.DTOs.PaymentDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Persistence.Concrates
{
    public class PaymentService : GenericService<Payment, PaymentCreateDTO, PaymentUpdateDTO, PaymentListDTO, PaymentDTO>, IPaymentService
    {
        private readonly IRepository<Payment> _repository;
        private readonly IMapper _mapper;

        public PaymentService(IRepository<Payment> repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentListDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var payments = await _repository.FindConditionAsync(p => p.AppUserId == userId && !p.IsDeleted);
            return _mapper.Map<IEnumerable<PaymentListDTO>>(payments);
        }

    }
}
