
using System.Diagnostics.Metrics;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.AddressDTOs
{
    public record AddressListDTO
    {
        public Guid Id { get; init; }
        public string AddressTitle { get; init; }
        public string AddressText { get; init; }
        public CityEnum City { get; init; }
        public string? Region { get; init; }
    }
}
