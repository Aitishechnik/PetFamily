using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileManagement.Providers;
using PetFamily.Core.FileManagment.Files;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernal;
using PetFamily.Volonteers.Application;
using PetFamily.Volonteers.Infrastructure.DbContexts;
using PetFamily.Volonteers.Infrastructure.MessageQueues;
using PetFamily.Volonteers.Infrastructure.Providers;
using PetFamily.Volonteers.Infrastructure.Repositories;
using MinioOptions = PetFamily.Core.Options.MinioOptions;

namespace PetFamily.Volonteers.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddVolonteerInfrastructure(
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
            services.AddScoped(_ =>
            new VolonteerWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
            services.AddScoped<IVolonteerReadDbContext, VolonteerReadDbContext>(_ =>
            new VolonteerReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }

        private static IServiceCollection AddHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<BackgroundServices.FilesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<IFilesCleanerService, Files.FilesCleanerBackgroundService.FilesCleanerService>();

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IVolonteersRepository, VolonteersRepository>();

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
