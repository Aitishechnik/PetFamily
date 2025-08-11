using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernal;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(breed => breed.Id);

            builder.Property(breed => breed.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);
        }
    }
}
