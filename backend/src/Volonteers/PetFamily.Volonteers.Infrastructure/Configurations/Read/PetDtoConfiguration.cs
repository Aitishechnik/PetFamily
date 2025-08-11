using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernal;
using System.Text.Json;

namespace PetFamily.Volonteers.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(pgi => pgi.Name)
                    .HasColumnName("name")
                    .IsRequired();

            builder.Property(pgi => pgi.Description)
                .HasColumnName("description")
                .IsRequired();

            builder.Property(pgi => pgi.Address)
                .HasColumnName("address")
                .IsRequired();

            builder.Property(pgi => pgi.OwnerPhoneNumber)
                .HasColumnName("owner_phone_number")
                .IsRequired();

            builder.Property(pgi => pgi.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired()
                .HasConversion(
                    dt => dt.ToUniversalTime(),
                    dt => DateTime.SpecifyKind(dt, DateTimeKind.Local));

            builder.Property(pgi => pgi.HelpStatus)
                .HasColumnName("help_status")
                .IsRequired()
                .HasConversion(
                    helpStatus => helpStatus.ToString(),
                    value => Utilities.Parse(value));

            builder.Property(pc => pc.Weight)
                    .HasColumnName("weight")
                    .IsRequired();

            builder.Property(pc => pc.Height)
                .HasColumnName("height")
                .IsRequired();

            builder.Property(pc => pc.Color)
                .HasColumnName("color")
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);

            builder.Property(ph => ph.HealthInfo)
                    .HasColumnName("health_info")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
            builder.Property(ph => ph.IsNeutered)
                .HasColumnName("is_neutered")
                .IsRequired();
            builder.Property(ph => ph.IsVaccinated)
                .HasColumnName("is_vaccinated")
                .IsRequired();

            builder.Property(s => s.SerialNumber)
                    .HasColumnName("serial_number")
                    .IsRequired();

            builder.Property(p => p.SpeciesId)
                    .HasColumnName("species_id")
                    .IsRequired();

            builder.Property(p => p.BreedId)
                .HasColumnName("breed_id")
                .IsRequired();

            builder.Property(i => i.DonationDetails)
                .HasConversion(
                    dd => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<DonationDetailsDto[]>(json, JsonSerializerOptions.Default)!);

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

            builder.Property(p => p.MainPhoto)
                .HasColumnName("main_photo")
                .IsRequired(false);
        }
    }
}