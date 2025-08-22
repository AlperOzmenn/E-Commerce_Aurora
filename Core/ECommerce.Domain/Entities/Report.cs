using ECommerce.Domain.Entities.Commons;

namespace ECommerce.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Mail { get; set; }
        public bool IsResolved { get; set; } = false;

    }
}
