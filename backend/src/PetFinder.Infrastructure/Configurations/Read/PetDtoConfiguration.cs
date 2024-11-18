using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Application.Dto;
using PetFinder.Infrastructure.Dto;

namespace PetFinder.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.ComplexProperty(p => p.Address, cpb =>
        {
            cpb.Property(a => a.Country)
                .HasColumnName("country");

            cpb.Property(a => a.City)
                .HasColumnName("city");

            cpb.Property(a => a.Street)
                .HasColumnName("street");

            cpb.Property(a => a.House)
                .HasColumnName("house");

            cpb.Property(a => a.Description)
                .HasColumnName("description");
        });

        builder.HasQueryFilter(p => p.IsDeleted == false);
    }
}