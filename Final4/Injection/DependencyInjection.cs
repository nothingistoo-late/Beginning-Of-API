using Final4.Data;
using Final4.IService;
using Final4.Service;
using Microsoft.EntityFrameworkCore;

namespace Final4.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IFlowerService, FlowerService>();
            return services;
        }
    }
}
