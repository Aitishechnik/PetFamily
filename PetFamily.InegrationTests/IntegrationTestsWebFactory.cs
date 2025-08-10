using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernal;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volonteers.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;
using PetFamily.Core.FileManagement.Providers;
using PetFamily.Volonteers.Domain.Entities;
using PetFamily.Volonteers.Domain.ValueObjects;
using PetFamily.Core.Dtos.Enums;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.InegrationTests
{
    public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private IFileProvider _fileProviderMock =
            Substitute.For<IFileProvider>();

        private Respawner _respawner = default!;

        private DbConnection _dbConnection = default!;

        private readonly PostgreSqlContainer _dbContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("test_db_name")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
        }

        protected virtual void ConfigureDefaultServices(IServiceCollection services)
        {
            services.RemoveAll<VolonteerWriteDbContext>();
            services.RemoveAll<SpeciesWriteDbContext>();
            services.RemoveAll<VolonteerReadDbContext>();
            services.RemoveAll<SpeciesReadDbContext>();
            services.RemoveAll<ISqlConnectionFactory>();
            services.RemoveAll<IFileProvider>();

            services.AddScoped(_ =>
            new VolonteerWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
            new SpeciesWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IVolonteerReadDbContext, VolonteerReadDbContext>(_ =>
            new VolonteerReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
            new SpeciesReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactoryTest(_dbContainer.GetConnectionString()));

            services.AddTransient(_ => _fileProviderMock);
        }

        public void SetupSuccessFileProviderMock(FilePath[] filePath)
        {
            _fileProviderMock
                .UploadFiles(Arg.Any<IReadOnlyList<FileDto>>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<IReadOnlyList<FilePath>, Error>(filePath));
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();
            var volonteerDbContext = scope.ServiceProvider.GetRequiredService<VolonteerWriteDbContext>();
            await volonteerDbContext.Database.EnsureDeletedAsync();
            await volonteerDbContext.Database.EnsureCreatedAsync();

            var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            await speciesDbContext.Database.EnsureDeletedAsync();
            await speciesDbContext.Database.EnsureCreatedAsync();

            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }

        public async Task<Guid> SeedVolonteer(
            VolonteerWriteDbContext _writeDbContext)
        {
            var volonteer = new Volonteer(
                Guid.NewGuid(),
                PersonalData.Create("Test Name", "test@test.test", "+123456789").Value,
                ProfessionalData.Create("Test description", 15).Value,
                new List<Pet>(),
                new List<SocialNetwork>(),
                new List<DonationDetails>());

            await _writeDbContext.Volonteers.AddAsync(volonteer);

            await _writeDbContext.SaveChangesAsync();

            return volonteer.Id;
        }

        public async Task<Guid> SeedPet(
            VolonteerWriteDbContext _writeDbContext,
            Guid volonteerId,
            Guid speciesId,
            Guid breedId)
        {
            var pet = new Pet(
                PetGeneralInfo.Create(
                    "Test Pet Name",
                    "Test Pet Description",
                    "Test Pet Address",
                    "Test Owner Adress",
                    new DateTime(2023, 1, 1),
                    HelpStatus.NeedsHelp).Value,
                PetCharacteristics.Create(
                    "Test Color",
                    15,
                    20).Value,
                PetHealthInfo.Create(
                    "Test Health Info",
                    true,
                    true).Value,
                new List<DonationDetails>([
                    DonationDetails.Create(
                        "Test Donation Details Name 1",
                        "Test Donation Details Descrition 1").Value,
                        DonationDetails.Create(
                        "Test Donation Details Name 1",
                        "Test Donation Details Descrition 1").Value]),
                PetType.Create(
                    speciesId,
                    breedId).Value);

            var volonteer =
                await _writeDbContext.Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            volonteer?.AddPet(pet);

            await _writeDbContext.SaveChangesAsync();

            return pet.Id;
        }

        public async Task<Guid> SeedSpecies(
            SpeciesWriteDbContext _writeDbContext)
        {
            var species = new Species.Domain.Entities.Species("Test Species");

            await _writeDbContext.Species.AddAsync(species);

            await _writeDbContext.SaveChangesAsync();

            return species.Id;
        }

        public async Task<Guid> SeedBreed(
            SpeciesWriteDbContext _writeDbContext,
            Guid speciesId)
        {
            var breed = new Breed("Test Breed");

            var species = await _writeDbContext
                .Species
                .FirstOrDefaultAsync(s => s.Id == speciesId);

            species?.AddBreeds([breed]);

            await _writeDbContext.SaveChangesAsync();

            return breed.Id;
        }

        public async Task<string[]> SeedPetPhotos(
            VolonteerWriteDbContext _writeDbContext,
            Guid volonteerId,
            Guid petId)
        {
            var volonteer =
                await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(
                    v => v.Id == volonteerId);

            var pet = volonteer?.Pets.FirstOrDefault(p => p.Id == petId);

            FilePath[] filePaths = new FilePath[3];

            for (int i = 0; i < filePaths.Length; i++)
                filePaths[i] = FilePath
                    .Create(
                    $"{volonteerId.ToString()}/{petId.ToString()}/{i}.jpg").Value;

            pet?.AddPhotos(filePaths);

            await _writeDbContext.SaveChangesAsync();

            SetupSuccessFileProviderMock(
                filePaths);

            return filePaths.Select(p => p.Path).ToArray();
        }
    }
}