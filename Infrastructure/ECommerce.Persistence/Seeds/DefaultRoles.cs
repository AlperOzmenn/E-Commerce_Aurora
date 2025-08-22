using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static void Seed(RoleManager<AppRole> roleManager)
        {
            string[] roles = { "Admin", "Seller", "Customer" };

            foreach (var role in roles)
            {
                var exists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();

                if (!exists)
                {
                    var result = roleManager.CreateAsync(new AppRole(role)
                    {
                        Description = $"{role} rolü sistem için varsayılan olarak oluşturuldu."
                    }).GetAwaiter().GetResult();

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"Rol eklenemedi: {role} | Hatalar: {errors}");
                    }
                }
            }
        }
    }
}
