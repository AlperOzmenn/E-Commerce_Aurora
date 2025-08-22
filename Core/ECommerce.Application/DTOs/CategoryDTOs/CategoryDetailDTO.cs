using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.CategoryDTOs
{
    public record CategoryDetailDTO : BaseDTO
    {
        [Display(Name = "Kategori Adı")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        [StringLength(300, ErrorMessage = "Açıklama en fazla 300 karakter olabilir.")]
        public string? Description { get; set; }

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
