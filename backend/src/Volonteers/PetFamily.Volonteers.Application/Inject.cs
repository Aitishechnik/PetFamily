using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volonteers.Application
{
    public static class Inject
    {
        public static IServiceCollection AddVolonteerApplication(this IServiceCollection services)
        {
            services
                .AddFileHandlers()
                .AddCommands()
                .AddQueries()
                .AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }

        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }

        private static IServiceCollection AddFileHandlers(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IFileHandler)))
                .AsSelf()
                .WithScopedLifetime());
        }
    }
}
