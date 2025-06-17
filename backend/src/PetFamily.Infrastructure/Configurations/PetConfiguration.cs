using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(pet => pet.Id);

            builder.ComplexProperty(pet => pet.PetGeneralInfo, pgib =>
            {
                pgib.Property(pgi => pgi.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                pgib.Property(pgi => pgi.Description)
                    .HasColumnName("description")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
                pgib.Property(pgi => pgi.Address)
                    .HasColumnName("address")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
                pgib.Property(pgi => pgi.OwnerPhoneNumber)
                    .HasColumnName("owner_phone_number")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);
                pgib.Property(pgi => pgi.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .IsRequired()
                    .HasConversion(
                        dt => dt.ToUniversalTime(),
                        dt => DateTime.SpecifyKind(dt, DateTimeKind.Local));
                pgib.Property(pgi => pgi.HelpStatus)
                    .HasColumnName("help_status")
                    .IsRequired()
                    .HasConversion(
                        helpStatus => helpStatus.ToString(),
                        value => Utilities.ParseHelpStatus(value));
            });

            builder.ComplexProperty(pet => pet.BreedAndSpecies, bs =>
            {
                bs.Property(b => b.Species)
                    .HasColumnName("species")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                bs.Property(b => b.Breed)
                    .HasColumnName("breed")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
            });

            builder.ComplexProperty(pet => pet.PetCharacteristics, pcb =>
            {
                pcb.Property(pc => pc.Weight)
                    .HasColumnName("weight")
                    .IsRequired();
                pcb.Property(pc => pc.Height)
                    .HasColumnName("height")
                    .IsRequired();
                pcb.Property(pc => pc.Color)
                    .HasColumnName("color")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
            });

            builder.ComplexProperty(pet => pet.PetHealthInfo, phi =>
            {
                phi.Property(ph => ph.HealthInfo)
                    .HasColumnName("health_info")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
                phi.Property(ph => ph.IsNeutered)
                    .HasColumnName("is_neutered")
                    .IsRequired();
                phi.Property(ph => ph.IsVaccinated)
                    .HasColumnName("is_vaccinated")
                    .IsRequired();

            });

            builder.OwnsOne(v => v.DonationDetails, dd =>
            {
                dd.ToJson("donation_details");

                dd.OwnsMany(dd => dd.DonationDetails, ddb =>
                {
                    ddb.Property(ddb => ddb.Name)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_NAME_LENGTH);

                    ddb.Property(ddb => ddb.Description)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
                });
            });

            builder.ComplexProperty(pet => pet.PetType, ptb =>
            {
                ptb.Property(p => p.SpeciesID)
                    .HasColumnName("species_id")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);

                ptb.Property(p => p.BreedID)
                    .HasColumnName("breed_id")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
            });

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasConversion(
                value => value.ToUniversalTime(),
                value => DateTime.SpecifyKind(value, DateTimeKind.Local));

            builder.HasOne<Volonteer>()
                .WithMany(v => v.Pets)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(v => v.IsDeleted)
                .HasColumnName("deleted");

            builder.Property(v => v.DeletionDate)
                .HasColumnName("deletion_date")
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Local) : null);
        }
    }
}