using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Persistence.Concrates
{
    public class GenericService<TEntity, TCreateDto, TUpdateDto, TListDto, TDto> 
        : IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto, TDto>
        where TEntity : IBaseEntity, new()
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenericService(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Ekleme
        public async Task AddAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        // Güncelleme
        public async Task UpdateAsync(TUpdateDto dto)
        {
            var idProp = dto!.GetType().GetProperty("Id");
            if (idProp == null)
                throw new Exception("Id bilgisi bulunamadı!");

            var id = (Guid)idProp.GetValue(dto)!;
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Güncellenecek kayıt bulunamadı!");

            _mapper.Map(dto, entity);
            entity.Update();
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        // Silme
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id, ignoreFilters: true);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        // Soft Delete
        public async Task SoftDeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                //entity.IsDeleted = true;
                entity.SoftDelete();
                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        // Restore
        public async Task RestoreAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id, ignoreFilters: true);
            if (entity != null)
            {
                entity.IsDeleted = false;
                entity.Update();
                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        // Bütün kayıtları getirme
        public async Task<IEnumerable<TListDto>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            var entities = await _repository.GetAllAsync(isTrack, ignoreFilters);
            return _mapper.Map<IEnumerable<TListDto>>(entities);
        }

        // Silinmiş kayıtları getirme
        public async Task<IEnumerable<TListDto>> GetAllDeletedAsync()
        {
            var entities = await _repository.FindConditionAsync(x => x.IsDeleted, false, true);
            return _mapper.Map<IEnumerable<TListDto>>(entities);
        }
        
        // Id ye göre kayıtları getirme
        public async Task<TDto?> GetByIdAsync(Guid id, bool ignoreFilters = false)
        {
            var entity = await _repository.GetByIdAsync(id, ignoreFilters);
            return _mapper.Map<TDto>(entity);
        }

        // Belirli bir Id'ye sahip Entity'yi getirir. DTO map edilmeden entity döner.
        public async Task<TEntity?> GetEntityByIdAsync(Guid id, bool ignoreFilters = false)
        {
            return await _repository.GetByIdAsync(id, ignoreFilters);
        }
    }
}