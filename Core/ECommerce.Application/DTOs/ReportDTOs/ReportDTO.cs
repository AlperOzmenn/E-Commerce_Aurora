using System.Net.Mail;

namespace ECommerce.Application.DTOs.ReportDTOs
{
    public record ReportDTO : BaseDTO
    {
        public string Subject { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Mail { get; set; }
        public DateTime CreatedDate { get; init; }
        public bool IsResolved { get; init; }
    }
}

