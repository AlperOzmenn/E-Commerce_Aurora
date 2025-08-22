using ECommerce.Domain.Entities.Commons;

namespace ECommerce.Application.Interfaces
{
    public interface IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto, TDto> where TEntity  : IBaseEntity, new()
    {
        Task AddAsync(TCreateDto dto);
        Task UpdateAsync(TUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task SoftDeleteAsync(Guid id);
        Task RestoreAsync(Guid id);
        Task<IEnumerable<TListDto>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false);
        Task<IEnumerable<TListDto>> GetAllDeletedAsync();
        Task<TDto?> GetByIdAsync(Guid id, bool ignoreFilters = false);
        Task<TEntity?> GetEntityByIdAsync(Guid id, bool ignoreFilters = false);
    }
}
