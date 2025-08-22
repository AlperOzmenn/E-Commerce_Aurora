using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistence.Seeds
{
    public static class DefaultAdmin
    {
        public static void Seed(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var adminEmail = "admin@mail.com";
            var adminPassword = "Admin123!";

            // Kullanıcı zaten var mı?
            var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();

            if (adminUser == null)
            {
                var user = new AppUser
                {
                    Name = "Admin",
                    Surname = "User",
                    Email = adminEmail,
                    UserName = adminEmail
                };

                var createResult = userManager.CreateAsync(user, adminPassword).GetAwaiter().GetResult();

                if (createResult.Succeeded)
                {
                    // Admin rolü yoksa ekle
                    var roleExists = roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult();
                    if (!roleExists)
                    {
                        roleManager.CreateAsync(new AppRole("Admin")
                        {
                            Description = "Sistemin en yetkili kullanıcısı."
                        }).GetAwaiter().GetResult();
                    }

                    // Rol ata
                    userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                    Console.WriteLine("Admin kullanıcı başarıyla oluşturuldu ve role atandı.");
                }
                else
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"Admin oluşturulamadı: {errors}");
                }
            }
        }
    }
}
