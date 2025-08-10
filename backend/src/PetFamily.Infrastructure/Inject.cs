using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetFamily.Application.Database;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Application.Messaging;
using PetFamily.Application.Species;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using static PetFamily.Infrastructure.Files.FilesCleanerBackgroundService;
using FileInfoPath = PetFamily.Application.FileManagment.Files.FileInfoPath;
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
                .AddDbContexts(configuration)
                .AddMinioCustom(configuration)
                .AddRepositories()
                .AddDatabase()
                .AddHostedServices()
                .AddMessageQueues()
                .AddServices();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            return services;
        }

        private static IServiceCollection AddMessageQueues(
            this IServiceCollection services)
        {
            services.AddSingleton<IMessageQueue<IEnumerable<FileInfoPath>>, MemoryMessageQueue<IEnumerable<FileInfoPath>>>();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

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
