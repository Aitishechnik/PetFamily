using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernal;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Species.Infrastructure.Repositories;

namespace PetFamily.Species.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddSpeciesInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddRepositories()
                .AddDatabase();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
            new SpeciesWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
            new SpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            return services;
        }

        private static IServiceCollection AddDatabase(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            return services;
        }
    }
}
