using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities
{
    public class Comment : BaseEntity
    {
        private string? _title;
        private string _content;
        public string? Title
        {
            get => _title;
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentException("Başlık en fazla 100 karakter olabilir.");

                _title = value;
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Yorum içeriği boş bırakılamaz.");

                if (value.Length > 1000)
                    throw new ArgumentException("Yorum içeriği en fazla 1000 karakter olabilir.");

                _content = value;
            }
        }
        public int CommentLike { get; set; }
        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }

        // Bu da bu yorumun altına gelen yanıtları tutar
        public ICollection<Comment> Replies { get; set; }
    }
}