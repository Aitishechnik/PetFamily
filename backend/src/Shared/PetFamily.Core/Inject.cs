using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Core
{
    public static class Inject
    {
        public static IServiceCollection AddFilesApplication(this IServiceCollection services)
        {
            services
                .AddFileHandlers();

            return services;
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
