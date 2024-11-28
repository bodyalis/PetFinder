using PetFinder.Application.Features.GetWithPagination;

namespace PetFinder.API.Controllers.Volunteer.Requests;

public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery() 
        => new GetVolunteersWithPaginationQuery(Page, PageSize);
}