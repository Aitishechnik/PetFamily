using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations
{
    internal class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(species => species.Id);

            builder.Property(species => species.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);

            builder.HasMany(species => species.Breeds)
                .WithOne()
                .HasForeignKey("species_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}