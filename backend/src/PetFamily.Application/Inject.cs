using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.UpdateMainInfo;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolonteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
