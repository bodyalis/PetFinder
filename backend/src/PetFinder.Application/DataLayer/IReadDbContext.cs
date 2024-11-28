using Microsoft.EntityFrameworkCore;
using PetFinder.Application.Dto;

namespace PetFinder.Application.DataLayer;

public interface IReadDbContext
{
    public DbSet<VolunteerDto> Volunteers { get; }
    public DbSet<PetDto> Pets { get; }
}