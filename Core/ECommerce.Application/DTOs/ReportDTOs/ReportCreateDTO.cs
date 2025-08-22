namespace ECommerce.Application.DTOs.ReportDTOs
{

    public record ReportCreateDTO : BaseDTO
    {
        // Report 
        public string Subject { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Mail { get; set; }
    }
}

