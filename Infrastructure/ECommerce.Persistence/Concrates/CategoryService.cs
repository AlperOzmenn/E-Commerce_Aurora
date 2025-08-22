using AutoMapper;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Persistence.Concrates
{
    public class CategoryService : GenericService<Category, CategoryCreateDTO, CategoryUpdateDTO, CategoryListDTO, CategoryDTO>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
        }
    }
}