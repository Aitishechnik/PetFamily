using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts;
using PetFamily.Species.Infrastructure;

namespace PetFamily.Species.Presentation;

public static class Inject
{
    public static IServiceCollection AddSpeciesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSpeciesInfrastructure(configuration)
            .AddSpeciesApplication()
            .AddSpeciesPresentation();

        return services;
    }

    private static IServiceCollection AddSpeciesPresentation(this IServiceCollection services)
    {
        services
            .AddContracts();

        return services;
    }

    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        return services.AddScoped<ISpeciesContract, SpeciesContract>();
    }

}