using AutoMapper;
using ECommerce.Application.DTOs.AddressDTOs;
using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.DTOs.PaymentDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.ViewModels;
using ECommerce.Application.ViewModels.AccountVMs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Entities.Products;


namespace ECommerce.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AppUser Mapping
            CreateMap<AppUser, AppUserCreateDTO>().ReverseMap();
            CreateMap<AppUser, AppUserListDTO>().ReverseMap();
            CreateMap<AppUser, AppUserDetailsDTO>().ReverseMap();
            CreateMap<AppUser, AppUserUpdateDTO>().ReverseMap();
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<AppUserDTO, AppUserUpdateDTO>().ReverseMap();
            CreateMap<AppUserCreateDTO, AppUser>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<AppUser, ProfileVM>().ReverseMap();

            // Product Mapping
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductListDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductDetailDTO>().ReverseMap();
            CreateMap<Product, ProductListDTO>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));

            // Category Mapping
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryListDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryDetailDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryDetailDTO>().ReverseMap();

            // Order Mapping
            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<Order, OrderListDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderDetailDTO>().ReverseMap(); 
            CreateMap<Order, OrderListDTO>().ReverseMap();

            // Seller Mapping
            CreateMap<Seller, SellerCreateDTO>().ReverseMap();
            CreateMap<Seller, SellerListDTO>().ReverseMap();
            CreateMap<Seller, SellerUpdateDTO>().ReverseMap();
            CreateMap<Seller, SellerDTO>().ReverseMap();
            CreateMap<Seller, SellerDetailDTO>().ReverseMap();
            CreateMap<Seller, SellerListDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AppUser.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ReverseMap();

            // Comment Mapping
            CreateMap<Comment, CommentCreateDTO>().ReverseMap();
            CreateMap<Comment, CommentListDTO>().ReverseMap();
            CreateMap<Comment, CommentUpdateDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDetailDTO>().ReverseMap();

            // Account Mapping
            CreateMap<AppUser, ProfileVM>().ReverseMap();

            //Register Mapping
            CreateMap<RegisterVM, AppUser>()
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<SellerRegisterVM, AppUser>()
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));


            //Address Mapping
            // CREATE
            CreateMap<AddressCreateDTO, Address>();

            // UPDATE
            CreateMap<AddressUpdateDTO, Address>();
            CreateMap<Address, AddressUpdateDTO>();

            // LIST
            CreateMap<Address, AddressListDTO>();

            // DETAIL
            CreateMap<Address, AddressDetailDTO>()
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src =>
                    src.AddressAppUsers.FirstOrDefault() != null
                        ? src.AddressAppUsers.FirstOrDefault().AppUserId
                        : Guid.Empty)) // veya null da olabilir, tercihe bağlı
                .ForMember(dest => dest.AppUserFullName, opt => opt.MapFrom(src =>
                    src.AddressAppUsers.FirstOrDefault() != null
                        ? src.AddressAppUsers.FirstOrDefault().AppUser.Name + " " +
                          src.AddressAppUsers.FirstOrDefault().AppUser.Surname
                        : null));

            // DETAIL -> ENTITY (isteğe bağlı, güncelleme vs. için)
            CreateMap<AddressDetailDTO, Address>();

            // Address -> Base DTO
            CreateMap<Address, AddressDTO>();

            // Report Mapping
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Report, ReportCreateDTO>().ReverseMap();
            CreateMap<Report, ReportUpdateDTO>().ReverseMap();
            CreateMap<Report, ReportListDTO>().ReverseMap();


            // Payment Mapping
            CreateMap<Payment, PaymentCreateDTO>().ReverseMap();
            CreateMap<Payment, PaymentListDTO>().ReverseMap();
        }
    }
}