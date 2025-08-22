using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryCreateDTO, CategoryUpdateDTO, CategoryListDTO, CategoryDTO>
    {
    }
}