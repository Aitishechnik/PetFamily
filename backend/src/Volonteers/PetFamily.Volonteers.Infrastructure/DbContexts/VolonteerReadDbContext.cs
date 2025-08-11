using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Infrastructure.DbContexts
{
    public class VolonteerReadDbContext(string connectionString) : DbContext, IVolonteerReadDbContext
    {
        public IQueryable<VolonteerDto> Volonteers => Set<VolonteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();

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
            modelBuilder.HasDefaultSchema("volonteers");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolonteerWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}