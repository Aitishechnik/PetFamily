using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;
using PetFamily.Application.Dtos;
using PetFamily.Contracts;

namespace PetFamily.Infrastructure.Configurations.Write
{
    internal class VolonteerConfiguration : IEntityTypeConfiguration<Volonteer>
    {
        public void Configure(EntityTypeBuilder<Volonteer> builder)
        {
            builder.ToTable("volonteers");

            builder.HasKey(v => v.Id);

            builder.ComplexProperty(v => v.PersonalData, pdb =>
            {
                pdb.Property(pd => pd.FullName)
                    .HasColumnName("full_name")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);

                pdb.Property(pd => pd.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_EMAIL_LENGTH);

                pdb.Property(pd => pd.PhoneNumber)
                    .HasColumnName("phone_number")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);
            });

            builder.ComplexProperty(v => v.ProfessionalData, pdb =>
            {
                pdb.Property(pd => pd.Description)
                    .HasColumnName("description")
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);

                pdb.Property(pd => pd.ExperienceInYears)
                    .HasColumnName("experience_in_years")
                    .IsRequired();
            });

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey(p => p.VolonteerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(v => v.Pets)
                .AutoInclude();

            builder.Property(v => v.SocialNetworks)
                .ValueObjectsCollectionJsonConversion(
                sn => new SocialNetworkDTO(sn.Name, sn.Link),
                dto => SocialNetwork.Create(dto.Name, dto.Link).Value)
                .HasColumnName("social_networks");

            builder.Property(v => v.DonationDetails)
                .ValueObjectsCollectionJsonConversion(
                sn => new DonationDetailsDTO(sn.Name, sn.Description),
                dto => DonationDetails.Create(dto.Name, dto.Description).Value)
                .HasColumnName("donation_details");

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
