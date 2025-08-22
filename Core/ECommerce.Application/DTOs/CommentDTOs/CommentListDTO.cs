using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.CommentDTOs
{
    public record CommentListDTO : BaseDTO
    {
        
        [Display(Name = "Yorum Başlığı")]
        public string? Title { get; init; }

        [Display(Name = "Yorum Özeti")]
        public string ContentSnippet { get; init; } // Yorumun kısa bir bölümünü gösterir.

        [Display(Name = "Kullanıcı ID")]
        public Guid AppUserId { get; init; }

        [Display(Name = "Kullanıcı Adı")]
        public string AppUserName { get; init; }

        [Display(Name = "Beğeni Sayısı")]
        public int CommentLike { get; init; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; init; }
    }
}
