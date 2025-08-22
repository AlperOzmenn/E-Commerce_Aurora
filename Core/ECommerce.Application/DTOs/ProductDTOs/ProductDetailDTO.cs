using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public record ProductDetailDTO :BaseDTO
    {
        [Display(Name = "Ürün Adı")]
        public string Name { get; init; }

        [Display(Name = "Ürün Açıklaması")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; init; }

        [Display(Name = "Fiyat")]
        [DataType(DataType.Currency)]
        public decimal Price { get; init; } = 0.0m;

        [Display(Name = "Ürün Görseli (URL)")]
        public string? ImageUrl { get; init; }

        [Display(Name = "Stok Miktarı")]
        public int Stock { get; init; }

        [Display(Name = "Oluşturulma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; init; }

        [Display(Name = "Güncellenme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; init; }

        [Display(Name = "Silinme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; init; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; init; } = false;
    }
}
