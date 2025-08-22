using ECommerce.Application.IoCs;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.IoCs;
using ECommerce.Persistence.Seeds;
using ECommerce.Infrastructure.Identity; // CustomIdentityErrorDescriber burada olmalý
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // IoC
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices();

            // DbContext
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            // Session
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            // Identity + Custom Error Describer
            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            })
            .AddErrorDescriber<CustomIdentityErrorDescriber>() // ?? Türkçe mesajlar
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //// Giriþ yetkisi yoksa burasý çalýþýr.
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Home/AccessDenied";
            //    options.AccessDeniedPath = "/Home/AccessDenied";

            //    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
            //    {
            //        OnRedirectToLogin = ctx =>
            //        {
            //            ctx.Response.Redirect("/Home/AccessDenied");
            //            return System.Threading.Tasks.Task.CompletedTask;
            //        }
            //    };
            //});

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Burayý AccessDenied olarak býrakýyoruz
                options.AccessDeniedPath = "/Home/AccessDenied";

                // Giriþ yapmamýþ kullanýcýlar için özel yönlendirme
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        // Eðer kullanýcý oturum açmamýþsa:
                        if (!ctx.HttpContext.User.Identity.IsAuthenticated)
                        {
                            ctx.Response.Redirect("/Account/Register");
                        }
                        else
                        {
                            // Kullanýcý giriþ yapmýþ ama yetkisi yoksa
                            ctx.Response.Redirect("/Home/AccessDenied");
                        }

                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            });


            // MVC
            builder.Services.AddControllersWithViews();

            // Build App
            var app = builder.Build();

            // SEED iþlemleri
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                DefaultRoles.Seed(roleManager);
                DefaultAdmin.Seed(userManager, roleManager);
            }

            //Session
            app.UseSession();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );

            });

            app.Run();
        }
    }
}