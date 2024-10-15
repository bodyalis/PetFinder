using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;

namespace PetFinder.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable(Constants.Breed.TableName);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));

        builder.Property(b => b.SpeciesId)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value))
            .HasColumnName("species_id")
            .IsRequired();

        builder.OwnsOne(b => b.Title, cpb =>
        {
            cpb.Property(t => t.Value)
                .HasColumnName("title")
                .HasMaxLength(Constants.Breed.MaxTitleLength)
                .IsRequired();
            
            cpb.HasIndex(bt => bt.Value).IsUnique();
        });

        builder.OwnsOne(b => b.Description, cpb =>
        {
            cpb.Property(t => t.Value)
                .HasColumnName("description")
                .HasMaxLength(Constants.Breed.MaxDescriptionLength)
                .IsRequired();
        });
    }
}