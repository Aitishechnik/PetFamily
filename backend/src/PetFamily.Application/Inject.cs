using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volonteers.CreateVolonteer;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolonteerHandler>();
            return services;
        }
    }
}
