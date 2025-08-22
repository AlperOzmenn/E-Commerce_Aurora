using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Fiyat bilgisi zorunludur.")]
        [Range(0.01, 1000000, ErrorMessage = "Fiyat 0.01 TL'den az olamaz.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Fiyat")]    //Negatif ya da sıfır olamaz. Para tipi olarak biçimlenebilir.
        public decimal UnitPrice { get; set; } = 0.0m;

        [Range(0, 100, ErrorMessage = "indirim 0 ile 100 arasında olmalıdır(%)")]
        public double? Discount { get; set; }


        //[Required(ErrorMessage = "Kategori bilgisi zorunludur.")]
        //[StringLength(50)]
        //[Display(Name = "Kategori")]
        //public string Category { get; set; }

        [Required(ErrorMessage = "Marka bilgisi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Marka")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Renk bilgisi zorunludur.")]
        [StringLength(30)]
        [Display(Name = "Renk")]
        public string Color { get; set; }

        [Display(Name = "Ürün Görseli (URL)")]
        public IFormFile? ImageUrl { get; init; }
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Stok bilgisi zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok değeri negatif olamaz.")]
        [Display(Name = "Stok Adedi")]
        public int Stock { get; set; }
        public Guid SellerId { get; set; }
        public Guid CategoryId { get; set; }
        // Additional properties can be added as needed
    }
}
