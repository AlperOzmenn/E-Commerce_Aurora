using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICommentService : IGenericService<Comment, CommentCreateDTO, CommentUpdateDTO, CommentListDTO, CommentDTO>
    {
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<List<Comment>> GetCommentsByProductIdAsync(Guid productId);
        Task<CommentCreateDTO> AddCommentAsync(CommentCreateDTO model, Guid userId);
    }
}
