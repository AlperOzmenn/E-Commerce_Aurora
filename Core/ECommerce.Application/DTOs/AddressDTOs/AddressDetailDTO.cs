
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.AddressDTOs
{
    public record AddressDetailDTO
    {
        public Guid Id { get; init; }

        public string AddressTitle { get; init; }

        public string AddressText { get; init; }

        public CityEnum City { get; init; }

        public string? Region { get; init; }

        public DateTime CreatedDate { get; init; }

        public DateTime? UpdatedDate { get; init; }

        public DateTime? DeletedDate { get; init; }

        public bool IsDeleted { get; init; }

        // Eğer kullanıcıyla eşleşiyorsa:
        public Guid? AppUserId { get; init; }
        public string? AppUserFullName { get; init; }
    }
}
