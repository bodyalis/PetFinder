using System.Collections;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Dto;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Models;
using PetFinder.Domain.Shared;

namespace PetFinder.Application.Features.GetWithPagination;

public class GetVolunteersWithPaginationHandler(
    IReadDbContext readDbContext,
    ILogger<GetVolunteersWithPaginationHandler> logger) : IHandler
{
    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await readDbContext.Volunteers.CountAsync(cancellationToken);

        var volunteers = await readDbContext.Volunteers.AsQueryable().GetWithPagination(
            query.Page,
            query.PageSize);
        
        return volunteers;
    }
}