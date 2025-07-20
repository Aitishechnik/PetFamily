using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetFamily.Application.Database;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Application.Messaging;
using PetFamily.Application.Species;
using PetFamily.Application.Volonteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using static PetFamily.Infrastructure.Files.FilesCleanerBackgroundService;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;
using MinioOptions = PetFamily.Infrastructure.Options.MinioOptions;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services
                .AddDbContexts()
                .AddMinioCustom(configuration)
                .AddRepositories()
                .AddDatabase()
                .AddHostedServices()
                .AddMessageQueues()
                .AddServices();

            return services;
        }

        private static IServiceCollection AddMessageQueues(
            this IServiceCollection services)
        {
            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, MemoryMessageQueue<IEnumerable<FileInfo>>>();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services)
        {
            services.AddScoped<WriteDbContext>();
            services.AddScoped<IReadDbContext, ReadDbContext>();

            return services;
        }

        private static IServiceCollection AddHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<FilesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<IFilesCleanerService, FilesCleanerService>();

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolonteersRepository, VolonteersRepository>();
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

        private static IServiceCollection AddMinioCustom(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMinio(options =>
            {
                var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                ?? throw new ApplicationException("Missing minio configuration");

                options.WithEndpoint(minioOptions.Endpoint);

                options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

                options.WithSSL(minioOptions.WithSsl);

            });

            services.AddScoped<IFileProvider, MinioProvider>();

            return services;
        }
    }
}
