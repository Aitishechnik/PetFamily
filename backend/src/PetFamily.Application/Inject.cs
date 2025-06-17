using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.UpdateDonationDetails;
using PetFamily.Application.Volonteers.UpdateMainInfo;
using PetFamily.Application.Volonteers.UpdateSocialNetworks;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolonteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateSocialNetworksHandler>();
            services.AddScoped<UpdateDonationDetailsHandler>();
            services.AddScoped<SoftDeleteVolonteerHandler>();
            services.AddScoped<HardDeleteVolonteerHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
