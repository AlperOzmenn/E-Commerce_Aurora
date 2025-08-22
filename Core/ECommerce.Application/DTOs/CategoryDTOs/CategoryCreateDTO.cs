using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.CategoryDTOs
{
    public record CategoryCreateDTO
    {

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Açıklama en fazla 300 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; } // boş geçilebilir, sınırlı uzunlukta

        // Additional properties can be added as needed
    }
}
