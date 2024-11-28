using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Infrastructure.Dto;

internal class AssistanceDetailsDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public  AssistanceDetails ToValueObjecct() 
        => AssistanceDetails.Create(title:Title, description:Description).Value;
}