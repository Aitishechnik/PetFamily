using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Volonteers.Domain.Entities;

namespace PetFamily.Volonteers.Infrastructure.DbContexts
{
    public class VolonteerWriteDbContext(string connectionString) : DbContext
    {
        public DbSet<Volonteer> Volonteers => Set<Volonteer>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("volonteers");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolonteerWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}