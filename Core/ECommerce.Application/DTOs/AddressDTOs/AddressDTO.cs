
using System.Diagnostics.Metrics;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.AddressDTOs
{
    public record AddressDTO
    {
        public Guid Id { get; init; }
        public string AddressTitle { get; init; }
        public string AddressText { get; init; }
        public CityEnum City { get; init; }
        public string? Region { get; init; }
        public DateTime CreatedDate { get; init; }
        public bool IsDeleted { get; init; }
    }
}
