using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Write
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
