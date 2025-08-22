using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public record ProductListDTO : BaseDTO
    {
        [Display(Name = "Ürün Adı")]
        public string Name { get; init; }

        [Display(Name = "Açıklama")]
        public string? Description { get; init; }

        [Display(Name = "Fiyat")]
        public decimal UnitPrice { get; init; }

        [Display(Name = "İndirim")]
        public double? Discount { get; init; }

        [Display(Name = "Son Fiyat")]
        public decimal FinalPrice { get; init; }

        //[Display(Name = "Görsel URL")]
        //public string? ImageUrl { get; init; }

        [Display(Name = "Stok")]
        public int Stock { get; init; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; init; }

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedDate { get; init; }

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; init; }

        [Display(Name = "Silinmiş Mi?")]
        public bool IsDeleted { get; init; } = false;

        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        [Display(Name = "Marka")]
        public string? Brand { get; init; }

        [Display(Name = "Renk")]
        public string? Color { get; init; }

        [Display(Name = "Resim")]
        public string? ImagePath { get; set; }
    }
}