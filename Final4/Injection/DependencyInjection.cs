using Final4.Data;
using Final4.IRepository;
using Final4.IService;
using Final4.Repository;
using Final4.Service;
using Final4.Service.Email;
using Microsoft.EntityFrameworkCore;

namespace Final4.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // add repository
            services.AddScoped<IFlowerRepository, FlowerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // add service
            services.AddScoped<IFlowerService, FlowerService>();

            // Đăng ký EmailService
            services.AddScoped<EmailService>();

            // Đăng ký Background Service
            services.AddHostedService<EmailBackgroundService>();

            return services;
        }
    }
}
