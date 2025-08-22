namespace ECommerce.Application.DTOs.ReportDTOs
{
    public record ReportUpdateDTO : BaseDTO
    {
        public string Subject { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Mail { get; set; }
        public bool IsResolved { get; init; }
    }
}

