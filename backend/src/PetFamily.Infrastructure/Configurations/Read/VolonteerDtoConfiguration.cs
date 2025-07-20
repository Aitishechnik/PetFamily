using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read
{
    internal class VolonteerDtoConfiguration : IEntityTypeConfiguration<VolonteerDto>
    {
        public void Configure(EntityTypeBuilder<VolonteerDto> builder)
        {
            builder.ToTable("volonteers");

            builder.HasKey(v => v.Id);

            builder.Property(pd => pd.FullName)
                    .HasColumnName("full_name")
                    .IsRequired();

            builder.Property(pd => pd.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(pd => pd.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired();

            builder.Property(pd => pd.Description)
                    .HasColumnName("description")
                    .IsRequired();

            builder.Property(pd => pd.ExperienceInYears)
                .HasColumnName("experience_in_years")
                .IsRequired();

            builder.Property(i => i.SocialNetworks)
                .HasConversion(
                sn => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);

            builder.Property(i => i.DonationDetails)
                .HasConversion(
                dd => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<DonationDetailsDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
