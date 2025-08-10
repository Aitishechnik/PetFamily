using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;

namespace PetFamily.Species.Infrastructure.Configurations.Read
{
    internal class BreedDtoConfigurtion : IEntityTypeConfiguration<BreedDto>
    {
        public void Configure(EntityTypeBuilder<BreedDto> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.HasOne<SpeciesDto>()
                .WithMany()
                .HasForeignKey(b => b.SpeciesId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.SpeciesId)
                .HasColumnName("species_id")
                .IsRequired();
        }
    }
}
