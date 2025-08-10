using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volonteers.Application;
using PetFamily.Volonteers.Contracts;
using PetFamily.Volonteers.Infrastructure;
using PetFamily.Volonteers.Presentation;

namespace PetFamily.Volonteers.Presentation;

public static class Inject
{
    public static IServiceCollection AddVolunteersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolonteerInfrastructure(configuration)
            .AddVolonteerApplication()
            .AddVolonteerPresentation();

        return services;
    }

    private static IServiceCollection AddVolonteerPresentation(this IServiceCollection services)
    {
        services
            .AddContracts();

        return services;
    }

    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        return services.AddScoped<IVolonteerContract, VolunteerContract>();
    }

}