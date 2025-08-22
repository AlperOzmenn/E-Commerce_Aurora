
using ECommerce.Application.DTOs.AddressDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IAddressService : IGenericService<Address, AddressCreateDTO, AddressUpdateDTO, AddressListDTO, AddressDTO>
    {



    }
}
