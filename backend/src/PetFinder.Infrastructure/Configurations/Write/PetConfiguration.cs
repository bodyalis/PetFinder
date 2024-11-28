using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;

namespace PetFinder.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.Name, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("name")
                .HasMaxLength(Constants.Pet.MaxNameLength)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.GeneralDescription, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("general_description")
                .HasMaxLength(Constants.Pet.MaxGeneralDescriptionLength)
                .IsRequired();
        });
        
        builder.ComplexProperty(p => p.Color, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("color")
                .HasMaxLength(Constants.Pet.MaxColorLength)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.HealthInformation, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("health_information")
                .HasMaxLength(Constants.Pet.MaxHealthInformationLength)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Address, pab =>
        {
            pab.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(Constants.Address.MaxCountryLength)
                .IsRequired();

            pab.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(Constants.Address.MaxCityLength)
                .IsRequired();

            pab.Property(a => a.Street)
                .HasColumnName("street")
                .HasMaxLength(Constants.Address.MaxStreetLength)
                .IsRequired();

            pab.Property(a => a.House)
                .HasColumnName("house")
                .HasMaxLength(Constants.Address.MaxHouseLength)
                .IsRequired();

            pab.Property(a => a.Description)
                .HasColumnName("description")
                .HasMaxLength(Constants.Address.MaxDescriptionLength);
        });

        builder.ComplexProperty(p => p.SpeciesBreedObject, sbob =>
        {
            sbob.Property(sbo => sbo.SpeciesId)
                .HasColumnName("species_id")
                .HasConversion(
                    speciesId => speciesId.Value,
                    value => SpeciesId.Create(value))
                .IsRequired();

            sbob.Property(sbo => sbo.BreedId)
                .HasColumnName("breed_id")
                .HasConversion(
                    breedId => breedId.Value,
                    value => BreedId.Create(value))
                .IsRequired();
        });
        
        builder.ComplexProperty(p => p.OwnerPhoneNumber, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("owner_phone")
                .HasMaxLength(Constants.Pet.MaxOwnerPhoneNumberLength)
                .IsRequired();
        });
        
        
        // todo add unique index for (pet_id, order_number)  
        builder.ComplexProperty(p => p.OrderNumber, cpb =>
        {
            cpb.Property(p => p.Value)
                .HasColumnName("order_number")
                .IsRequired();
        });

        builder.Property(p => p.Weight)
            .HasColumnName("weight")
            .IsRequired();

        builder.Property(p => p.Height)
            .HasColumnName("height")
            .IsRequired();
        
        builder.Property(p => p.BirthDate)
            .HasColumnName("birth_date")
            .IsRequired();

        builder.Property(p => p.IsCastrated)
            .HasColumnName("is_castrated")
            .IsRequired();

        builder.Property(p => p.IsVaccinated)
            .HasColumnName("is_vaccinated")
            .IsRequired();

        builder.Property(p => p.HelpStatus)
            .HasColumnName("help_status")
            .IsRequired();

        builder.HasMany(p => p.Photos)
            .WithOne()
            .HasForeignKey("pet_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(
            name: Constants.Pet.TableName,
            buildAction: t =>
            {
                t.HasCheckConstraint("CK_Pet_weight", "\"weight\" > 0");
                t.HasCheckConstraint("CK_Pet_height", "\"height\" > 0");
            });

        builder.HasQueryFilter(pet => pet.IsDeleted == false);
    }
}