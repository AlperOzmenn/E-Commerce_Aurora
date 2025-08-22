using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public record ProductUpdateDTO : BaseDTO
    {
        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; init; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; init; }

        [Required(ErrorMessage = "Fiyat alanı boş bırakılamaz.")]
        [Range(0.01, 1000000, ErrorMessage = "Fiyat en az 0.01 TL olmalıdır.")]
        [Display(Name = "Fiyat")]
        [DataType(DataType.Currency)]
        public decimal Price { get; init; } = 0.0m;

        [Display(Name = "Ürün Görseli (URL)")]
        public string? ImageUrl { get; init; }

        [Required(ErrorMessage = "Stok miktarı girilmelidir.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok değeri negatif olamaz.")]
        [Display(Name = "Stok")]
        public int Stock { get; init; }
        // Inherits properties from ProductCreateDTO and adds Id for updates
    }
}
