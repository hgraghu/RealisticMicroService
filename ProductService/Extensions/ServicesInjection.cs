using ProductService.Interfaces;
using ProductService.Repositories;

namespace AuthenticationService.Extensions
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
