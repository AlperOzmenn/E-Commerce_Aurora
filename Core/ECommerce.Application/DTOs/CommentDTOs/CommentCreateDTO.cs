using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.CommentDTOs
{
    public record CommentCreateDTO
    {
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        [Display(Name = "Yorum Başlığı")]
        public string? Title { get; init; }

        [Required(ErrorMessage = "Yorum içeriği boş bırakılamaz.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Yorum en az 5, en fazla 1000 karakter olmalıdır.")]
        [Display(Name = "Yorum İçeriği")]
        public string Content { get; init; }

        [Required(ErrorMessage = "Kullanıcı bilgisi zorunludur.")]
        [Display(Name = "Kullanıcı")]
        public Guid AppUserId { get; set; }

        [Required(ErrorMessage = "Ürün bilgisi zorunludur.")]
        [Display(Name = "Ürün")]
        public Guid ProductId { get; init; }

        [Display(Name = "Yanıtlanan Yorum")]
        public Guid? ParentCommentId { get; init; }
    }
}