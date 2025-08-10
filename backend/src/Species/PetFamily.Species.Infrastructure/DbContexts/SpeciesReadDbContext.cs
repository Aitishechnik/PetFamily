using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Species.Infrastructure.DbContexts
{
    public class SpeciesReadDbContext(string connectionString) : DbContext, ISpeciesReadDbContext
    {
        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
        public IQueryable<BreedDto> Breeds => Set<BreedDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}