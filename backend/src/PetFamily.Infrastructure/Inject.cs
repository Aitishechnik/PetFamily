using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volonteers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<IVolonteersRepository, VolonteersRepository>();
            return services;
        }
    }
}
