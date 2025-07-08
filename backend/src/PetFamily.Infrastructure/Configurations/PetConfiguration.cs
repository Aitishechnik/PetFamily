using System.Text.Json;
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

            builder.ComplexProperty(pet => pet.SerialNumber, snb =>
            {
                snb.Property(s => s.Value)
                    .HasColumnName("serial_number")
                    .IsRequired();
            });

            builder.Property(p => p.DonationDetails)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<List<DonationDetails>>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb")
                .HasColumnName("donation_details")
                .IsRequired();

            builder.Property(p => p.PetPhotos)
                .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<FilePath>>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb")
                .HasColumnName("pet_photos")
                .IsRequired();

            builder.ComplexProperty(pet => pet.PetType, ptb =>
            {
                ptb.Property(p => p.SpeciesID)
                    .HasColumnName("species_id")
                    .IsRequired();

                ptb.Property(p => p.BreedID)
                    .HasColumnName("breed_id")
                    .IsRequired();
            });

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasConversion(
                value => value.ToUniversalTime(),
                value => DateTime.SpecifyKind(value, DateTimeKind.Local));

            builder.Property(p => p.VolonteerId)
                .HasColumnName("volonteer_id")
                .IsRequired();

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