using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.ViewModels
{
    public class ProductDetailVM
    {
        public ProductDTO Product { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
