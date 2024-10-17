using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFinder.Application.Features;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.Models;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Infrastructure.Repositories;

public class VolunteerRepository(ApplicationDbContext dbContext) : IVolunteerRepository
{
    public VolunteerId Add(Volunteer volunteer)
    {
        dbContext.Add(volunteer);
        return volunteer.Id;
    }

    public void Save(Volunteer volunteer) 
        => dbContext.Attach(volunteer);

    public void Delete(Volunteer volunteer)
        => dbContext.Remove(volunteer);
    
    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        var volunteer = await GetBy(
            v => v.Id == volunteerId,
            cancellationToken);

        return volunteer is null
            ? Errors.General.RecordNotFound(nameof(Volunteer), nameof(VolunteerId))
            : volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByEmail(Email email,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await GetBy(
            v => v.Email == email,
            cancellationToken);

        return volunteer is null
            ? Errors.General.RecordNotFound(nameof(Volunteer), nameof(Email))
            : volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await GetBy(
            v => v.PhoneNumber == phoneNumber,
            cancellationToken);

        return volunteer is null
            ? Errors.General.RecordNotFound(nameof(Volunteer), nameof(PhoneNumber))
            : volunteer;
    }

    public async Task<bool> CheckPhoneNumberForExists(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
        => await dbContext.Volunteers.AnyAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

    public async Task<bool> CheckEmailForExists(Email email, CancellationToken cancellationToken = default)
        => await dbContext.Volunteers.AnyAsync(v => v.Email == email, cancellationToken);
     
    
    private Task<Volunteer?> GetBy(Expression<Func<Volunteer, bool>> expression,
        CancellationToken cancellationToken)
    {
        return dbContext.Volunteers
            .Include(v => v.Pets)
            .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(expression, cancellationToken);
    }
}