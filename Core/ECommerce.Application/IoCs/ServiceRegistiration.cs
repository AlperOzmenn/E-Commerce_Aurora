using ECommerce.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.IoCs
{
    public static class ServiceRegistiration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
    }
}
