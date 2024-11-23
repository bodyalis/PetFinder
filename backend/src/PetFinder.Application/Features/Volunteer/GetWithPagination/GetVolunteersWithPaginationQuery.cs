using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.GetWithPagination;

public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;