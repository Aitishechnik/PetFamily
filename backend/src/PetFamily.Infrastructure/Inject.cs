﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetFamily.Application.FileManagement.Providers;
using PetFamily.Application.Messaging;
using PetFamily.Application.Species;
using PetFamily.Application.Volonteers;
using PetFamily.Infrastructure.BackgroundServices;
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
            services.AddScoped<AppDbContext>();
            services.AddScoped<IVolonteersRepository, VolonteersRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMinioCustom(configuration);

            services.AddHostedService<FilesCleanerBackgroundService>();

            services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, MemoryMessageQueue<IEnumerable<FileInfo>>>();

            services.AddScoped<IFilesCleanerService, FilesCleanerService>();

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
