using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.CommentDTOs
{
    public record CommentDetailDTO : BaseDTO
    {
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        [Display(Name = "Yorum Başlığı")]
        public string? Title { get; init; }

        [Required(ErrorMessage = "Yorum içeriği boş bırakılamaz.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Yorum en az 5, en fazla 1000 karakter olmalıdır.")]
        [Display(Name = "Yorum İçeriği")]
        [DataType(DataType.MultilineText)]  //Content alanı formda geniş textarea olarak gösterilebilir.
        public string Content { get; init; }

        [Display(Name = "Beğeni Sayısı")]
        public int CommentLike { get; init; }

        [Display(Name = "Kullanıcı ID")]
        public Guid AppUserId { get; init; }

        [Display(Name = "Kullanıcı Adı")]
        public string AppUserName { get; init; }

        [Display(Name = "Ürün ID")]
        public Guid ProductId { get; init; }

        [Display(Name = "Cevap Verilen Yorum ID")]
        public Guid? ParentCommentId { get; init; }

        [Display(Name = "Yorum Yanıtları")]
        public List<CommentDetailDTO> Replies { get; init; } = new List<CommentDetailDTO>();
    }
}
