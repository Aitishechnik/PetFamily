using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PetFamily.Species.Infrastructure.DbContexts
{
    public class SpeciesWriteDbContext(string connectionString) : DbContext
    {
        public DbSet<Domain.Entities.Species> Species => Set<Domain.Entities.Species>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("species");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}