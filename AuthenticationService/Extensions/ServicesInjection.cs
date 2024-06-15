using AuthenticationService.Interfaces;
using AuthenticationService.Services;

namespace AuthenticationService.Extensions
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, SendMailService>();
            return services;
        }
    }
}
