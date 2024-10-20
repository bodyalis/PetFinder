using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.Models;

namespace PetFinder.Infrastructure.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetPhotoId.Create(value));

        builder.ComplexProperty(p => p.FileInfo, cpb =>
        {
            cpb.Property(p => p.Path)
                .HasColumnName("path")
                .HasMaxLength(Constants.PetPhoto.MaxPathLength)
                .IsRequired();

            cpb.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(Constants.PetPhoto.MaxNameLength)
                .IsRequired();
        });

        builder.Property(p => p.IsMain)
            .HasColumnName("is_main")
            .IsRequired();

        builder.ToTable(
            name: Constants.PetPhoto.TableName
        );

        builder.HasQueryFilter(photo => photo.IsDeleted == false);
    }
}