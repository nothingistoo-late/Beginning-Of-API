using Final4.Data;
using Final4.IRepository;
using Final4.IService;
using Final4.Repository;
using Final4.Service;
using Final4.Service.Email;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Final4.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // add repository
            services.AddScoped<IFlowerRepository, FlowerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // add service
            services.AddScoped<IFlowerService, FlowerService>();
            // add mapper 
            services.AddAutoMapper(typeof(MappingProfile));

            // Register EmailService
            services.AddScoped<EmailService>();

            // Register Background Service
            services.AddHostedService<EmailBackgroundService>();

            return services;
        }
    }
}