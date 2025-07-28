using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write
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

            builder.Property(v => v.DonationDetails)
                .ValueObjectsCollectionJsonConversion(
                sn => new DonationDetailsDto()
                { Name = sn.Name, Description = sn.Description },
                dto => DonationDetails.Create(dto.Name, dto.Description).Value)
                .HasColumnName("donation_details");

            builder.OwnsMany(p => p.PetPhotos, ppb =>
            {
                ppb.ToJson("pet_photos");

                ppb.Property(pp => pp.Path)
                    .HasColumnName("path")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LINK_LENGTH);
            });

            builder.ComplexProperty(pet => pet.PetType, ptb =>
            {
                ptb.Property(p => p.SpeciesId)
                    .HasColumnName("species_id")
                    .IsRequired();

                ptb.Property(p => p.BreedId)
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

            builder.Property(p => p.IsDeleted)
                .HasColumnName("deleted");

            builder.Property(p => p.DeletionDate)
                .HasColumnName("deletion_date")
                .HasConversion(
                    p => p.HasValue ? p.Value.ToUniversalTime() : (DateTime?)null,
                    p => p.HasValue ? DateTime.SpecifyKind(p.Value, DateTimeKind.Local) : null);

            builder.OwnsOne(p => p.MainPhoto, mpb =>
            {
                mpb.Property(mp => mp.Path)
                    .HasColumnName("main_photo")
                    .HasMaxLength(Constants.MAX_LINK_LENGTH)
                    .IsRequired(false);
            });
        }
    }
}