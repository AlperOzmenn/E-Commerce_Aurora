using ECommerce.Application.DTOs.AppUserDTOs;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.DTOs.CommentDTOs;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.DTOs.PaymentDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.DTOs.ReportDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Entities.Products;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Persistence.Concrates;
using ECommerce.Persistence.Repositories;
using ECommerce.Persistence.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persistence.IoCs
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            // Registering the Services
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IBasketSessionService, BasketSessionService>();
            services.AddScoped<IPaymentService, PaymentService>();


            services.AddScoped<IGenericService<AppUser, AppUserCreateDTO, AppUserUpdateDTO, AppUserListDTO, AppUserDTO>,
                                GenericService<AppUser, AppUserCreateDTO, AppUserUpdateDTO, AppUserListDTO, AppUserDTO>>();

            services.AddScoped<IGenericService<Product, ProductCreateDTO, ProductUpdateDTO, ProductListDTO, ProductDTO>,
                                GenericService<Product, ProductCreateDTO, ProductUpdateDTO, ProductListDTO, ProductDTO>>();

            services.AddScoped<IGenericService<Category, CategoryCreateDTO, CategoryUpdateDTO, CategoryListDTO, CategoryDTO>,
                                GenericService<Category, CategoryCreateDTO, CategoryUpdateDTO, CategoryListDTO, CategoryDTO>>();

            services.AddScoped<IGenericService<Order, OrderCreateDTO, OrderUpdateDTO, OrderListDTO, OrderDTO>,
                                GenericService<Order, OrderCreateDTO, OrderUpdateDTO, OrderListDTO, OrderDTO>>();

            services.AddScoped<IGenericService<Comment, CommentCreateDTO, CommentUpdateDTO, CommentListDTO, CommentDTO>,
                                GenericService<Comment, CommentCreateDTO, CommentUpdateDTO, CommentListDTO, CommentDTO>>();

            services.AddScoped<IGenericService<Seller, SellerCreateDTO, SellerUpdateDTO, SellerListDTO, SellerDTO>,
                                GenericService<Seller, SellerCreateDTO, SellerUpdateDTO, SellerListDTO, SellerDTO>>();

            services.AddScoped<IGenericService<Payment, PaymentCreateDTO, PaymentUpdateDTO, PaymentListDTO, PaymentDTO>,
                                GenericService<Payment, PaymentCreateDTO, PaymentUpdateDTO, PaymentListDTO, PaymentDTO>>();

            // Registering the Repositorys
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // IRepository
            services.AddScoped<IRepository<AppUser>, EfRepository<AppUser>>();
            services.AddScoped<IRepository<Product>, EfRepository<Product>>();
            services.AddScoped<IRepository<Category>, EfRepository<Category>>();
            services.AddScoped<IRepository<Order>, EfRepository<Order>>();
            services.AddScoped<IRepository<Comment>, EfRepository<Comment>>();
            services.AddScoped<IRepository<Seller>, EfRepository<Seller>>();
            services.AddScoped<IRepository<Address>, EfRepository<Address>>();
            services.AddScoped<IRepository<AddressAppUser>, EfRepository<AddressAppUser>>();
            services.AddScoped<IRepository<Payment>, EfRepository<Payment>>();

            // Address Service and Repository
            services.AddScoped<IRepository<Address>, EfRepository<Address>>();
            services.AddScoped<IRepository<AddressAppUser>, EfRepository<AddressAppUser>>();

            // Report Service and Repository
            services.AddScoped<IGenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO>,
                   GenericService<Report, ReportCreateDTO, ReportUpdateDTO, ReportListDTO, ReportDTO>>();

            services.AddScoped<IRepository<Report>, EfRepository<Report>>();

        }
    }
}
