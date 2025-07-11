﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations
{
    internal class VolonteerConfiguration : IEntityTypeConfiguration<Volonteer>
    {
        public void Configure(EntityTypeBuilder<Volonteer> builder)
        {
            builder.ToTable("volonteers");

            builder.HasKey(v => v.Id)
                .HasName("pk_volonteers");

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

            builder.OwnsOne(v => v.SocialNetworks, sns =>
            {
                sns.ToJson("social_networks");

                sns.OwnsMany(sns => sns.SocialNetworks, snb =>
                {
                    snb.Property(snb => snb.Name)
                        .HasColumnName("name")
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_NAME_LENGTH);

                    snb.Property(snb => snb.Link)
                        .HasColumnName("link")
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LINK_LENGTH);
                });
            });

            builder.OwnsOne(v => v.DonationDetails, dd =>
            {
                dd.ToJson("donation_details");

                dd.OwnsMany(dd => dd.DonationDetails, ddb =>
                {
                    ddb.Property(ddb => ddb.Name)
                        .HasColumnName("name")
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_NAME_LENGTH);

                    ddb.Property(ddb => ddb.Description)
                        .HasColumnName("description")
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH);
                });
            });

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
