using UserService.Interfaces;
using UserService.Services;

namespace UserService.Extensions
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IMailService, SendMailService>();
            return services;
        }
    }
}
