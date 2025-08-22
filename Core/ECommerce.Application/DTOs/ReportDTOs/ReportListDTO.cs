namespace ECommerce.Application.DTOs.ReportDTOs
{
    public record ReportListDTO : BaseDTO
    {
        public string Subject { get; init; } = string.Empty;
        public string Mail { get; init; }
        public DateTime CreatedDate { get; init; }
        public bool IsResolved { get; init; }
    }
}

