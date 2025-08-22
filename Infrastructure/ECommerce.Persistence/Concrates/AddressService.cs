
using AutoMapper;
using ECommerce.Application.DTOs.AddressDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Persistence.Concrates
{
    public class AddressService : GenericService<Address, AddressCreateDTO, AddressUpdateDTO, AddressListDTO, AddressDTO>, IAddressService
    {
        public AddressService(IRepository<Address> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
    }
}
