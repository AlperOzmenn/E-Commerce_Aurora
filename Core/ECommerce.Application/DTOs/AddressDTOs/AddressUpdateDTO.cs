
using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.AddressDTOs
{
    public record AddressUpdateDTO
    {
        [Required]
        public Guid Id { get; init; }

        [Required(ErrorMessage = "Adres başlığı zorunludur.")]
        [StringLength(100)]
        public string AddressTitle { get; init; }

        [Required(ErrorMessage = "Adres metni zorunludur.")]
        [StringLength(500)]
        public string AddressText { get; init; }

        [Required(ErrorMessage = "Şehir bilgisi zorunludur.")]
        public CityEnum City { get; init; }

        [StringLength(100)]
        public string? Region { get; init; }
    }
}
