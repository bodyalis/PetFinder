using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Application.Dto;
using PetFinder.Infrastructure.Dto;

namespace PetFinder.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.HasKey(v => v.Id);
        builder.ComplexProperty(v => v.PersonName, cpb =>
        {
            cpb.Property(p => p.FirstName).HasColumnName("first_name");
            cpb.Property(p => p.MiddleName).HasColumnName("middle_name");
            cpb.Property(p => p.LastName).HasColumnName("last_name");
        });

        builder.OwnsOne(v => v.AssistanceDetailsDtos,
            action => action.ToJson());
        builder.OwnsOne(v => v.SocialNetworks,
            action => action.ToJson());

        builder.HasQueryFilter(v => v.IsDeleted == false);
    }
}