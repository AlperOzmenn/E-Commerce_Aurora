using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService : IGenericService<Product, ProductCreateDTO, ProductUpdateDTO, ProductListDTO, ProductDTO>
    {
        Task<IEnumerable<Product>> GetProductsBySellerIdAsync(Guid sellerId);
        Task<List<Product>> GetProductsBySellerListIdAsync(Guid sellerId);
        Task<IEnumerable<ProductListDTO>> GetAllWithCategoryAsync();
        
    }
}
