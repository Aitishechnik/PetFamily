using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernal;

namespace PetFamily.Species.Infrastructure.Configurations.Write
{
    internal class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Entities.Species>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(species => species.Id);

            builder.Property(species => species.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);

            builder.HasMany(species => species.Breeds)
                .WithOne()
                .HasForeignKey(b => b.SpeciesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}