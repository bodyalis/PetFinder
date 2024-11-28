using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Application.Dto;
using PetFinder.Domain.SharedKernel;
using PetFinder.Infrastructure.Dto;

namespace PetFinder.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable(Constants.Pet.TableName);
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasColumnName("name");
        
        builder.Property(p => p.GeneralDescription)
            .HasColumnName("general_description");
        
        builder.Property(p => p.Color)
            .HasColumnName("color");
        
        builder.Property(p => p.HealthInformation)
            .HasColumnName("health_information");

        builder.Property(p => p.SpeciesId)
            .HasColumnName("species_id");

        builder.Property(p => p.BreedId)
            .HasColumnName("breed_id");

        builder.Property(p => p.OwnerPhoneNumber)
            .HasColumnName("owner_phone");
        
        builder.Property(p => p.OrderNumber)
            .HasColumnName("order_number");
        
        builder.Property(p => p.Weight)
            .HasColumnName("weight");
        
        builder.Property(p => p.Height)
            .HasColumnName("height");

        builder.Property(p => p.BirthDate)
            .HasColumnName("birth_date");
        
        builder.Property(p => p.IsCastrated)
            .HasColumnName("is_castrated");

        builder.Property(p => p.IsVaccinated)
            .HasColumnName("is_vaccinated");

        builder.Property(p => p.HelpStatus)
            .HasColumnName("help_status");
        
        // todo Описать связь с фотографиями, после добавления ДТО для них
        
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