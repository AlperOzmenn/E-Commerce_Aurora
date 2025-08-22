using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Commons;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressAppUser> AddressAppUsers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.OrderDetail)
                .WithMany() // Eğer OrderDetail’in Payments koleksiyonu yoksa böyle bırak
                .HasForeignKey(p => p.OrderDetailId)
                .OnDelete(DeleteBehavior.Restrict); // Burada cascade delete kapandı

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.AppUser)
                .WithMany() // Eğer AppUser’in Payments koleksiyonu yoksa
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); // Gerekirse AppUser için de kapatabilirsin

            modelBuilder.Entity<AppUserRole>(entity =>
            {
                // entity.HasKey(ur => new { ur.UserId, ur.RoleId }); // Kaldır ya da yorumla

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Soft delete olanları getirmez.
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(item.ClrType))
                {
                    var parameter = Expression.Parameter(item.ClrType, "e");
                    var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(IBaseEntity.IsDeleted)),
                            Expression.Constant(false)
                        ), parameter);

                    modelBuilder.Entity(item.ClrType).HasQueryFilter(filter);
                }
            }

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Seller>().ToTable("Sellers");

            modelBuilder.Entity<AddressAppUser>().HasKey(a => new {
                a.AddressId,
                a.AppUserId
            });

            // OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Order silinince detayları da sil

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Product silinince OrderDetail varsa hata ver
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerce.MVC"));
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}