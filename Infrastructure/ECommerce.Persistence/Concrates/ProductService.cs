using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Persistence.Repositories;
using ECommerce.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Concrates
{
    public class ProductService : GenericService<Product, ProductCreateDTO, ProductUpdateDTO, ProductListDTO, ProductDTO>, IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductListDTO>> GetAllWithCategoryAsync()
        {
            return await _unitOfWork.GetRepository<Product>().GetFilteredListAsync(
                select: p => new ProductListDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    UnitPrice = p.UnitPrice,
                    FinalPrice = p.FinalPrice,
                    Discount = p.Discount,
                    Brand = p.Brand,
                    Stock = p.Stock,
                    ImagePath = p.ImageUrl,
                    CategoryName = p.Category.Name // Include yapılmazsa bu null olur
                },
                where: p => !p.IsDeleted,
                join: p => p.Include(x => x.Category) // << Bunu eklemen şart
            );
        }

        public async Task<IEnumerable<Product>> GetProductsBySellerIdAsync(Guid sellerId)
        {
            return await _repository.FindConditionAsync(x => x.SellerId == sellerId, isTrack: false);
        }

        public async Task<List<Product>> GetProductsBySellerListIdAsync(Guid sellerId)
        {
            return (await _repository.FindConditionAsync(x => x.SellerId == sellerId, isTrack: false)).ToList();
        }
    }
}