using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using PetFinder.Application.Dto;
using PetFinder.Domain.SharedKernel;
using PetFinder.Infrastructure.Dto;
using AssistanceDetailsDto = PetFinder.Application.Dto.AssistanceDetailsDto;
using SocialNetworkDto = PetFinder.Application.Dto.SocialNetworkDto;

namespace PetFinder.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable(Constants.Volunteer.TableName);
        
        builder.HasKey(v => v.Id);
        
        builder.ComplexProperty(v => v.PersonName, cpb =>
        {
            cpb.Property(p => p.FirstName).HasColumnName("first_name");
            cpb.Property(p => p.MiddleName).HasColumnName("middle_name");
            cpb.Property(p => p.LastName).HasColumnName("last_name");
        });

        builder.Property(v => v.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(v => v.Email)
            .HasColumnName("email");

        builder.Property(v => v.Description)
            .HasColumnName("description");
        
        builder.Property(v => v.ExperienceYears)
            .HasColumnName("experience_years");
        
        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasColumnName("is_deleted");
        
        builder.Property(v => v.AssistanceDetailsDtos)
            .HasConversion(
                a => JsonSerializer.Serialize(a, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<AssistanceDetailsDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("assistance_details");

        
        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                a => JsonSerializer.Serialize(a, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetworkDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("social_networks");
        
        builder.HasQueryFilter(v => v.IsDeleted == false);
    }
}