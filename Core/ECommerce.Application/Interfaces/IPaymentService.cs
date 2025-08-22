using ECommerce.Application.DTOs.PaymentDTOs;
using ECommerce.Domain.Entities.Payments;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentService : IGenericService<Payment, PaymentCreateDTO, PaymentUpdateDTO, PaymentListDTO, PaymentDTO>
    {
        Task<IEnumerable<PaymentListDTO>> GetAllByUserIdAsync(Guid userId);
    }
}