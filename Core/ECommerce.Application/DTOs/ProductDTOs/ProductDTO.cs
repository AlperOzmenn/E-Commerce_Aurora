using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public record ProductDTO : BaseDTO
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; init; }

        [Display(Name = "Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; init; }


        [Required(ErrorMessage = "Fiyat bilgisi zorunludur.")]
        [Range(0.01, 1000000, ErrorMessage = "Fiyat 0.01 TL'den az olamaz.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Fiyat")]    //Negatif ya da sıfır olamaz. Para tipi olarak biçimlenebilir.
        public decimal Price { get; init; } = 0.0m;

        [Display(Name = "Görsel URL")]
        public string ImageUrl { get; init; }

        [Required(ErrorMessage = "Stok bilgisi zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok değeri negatif olamaz.")]
        [Display(Name = "Stok Adedi")]
        public int Stock { get; init; }

        [Display(Name = "Kategori ID")]
        public string CategoryId { get; init; }

        [Display(Name = "Oluşturulma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; init; } = DateTime.Now;

        [Display(Name = "Güncellenme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; init; }

        [Display(Name = "Silinme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; init; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; init; } = false;

        public Guid SellerId { get; init; }

        public string Brand { get; set; }
        public string Color { get; set; }
        public string CategoryName { get; set; }  // Category.Name
        public decimal UnitPrice { get; set; }
        public double? Discount { get; set; }
        public decimal FinalPrice => Discount.HasValue
            ? UnitPrice - (UnitPrice * ((decimal)Discount.Value / 100))
            : UnitPrice;
    }
}
