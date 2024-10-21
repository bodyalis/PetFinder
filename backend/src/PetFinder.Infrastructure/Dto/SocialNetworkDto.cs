using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Infrastructure.Dto;

public class SocialNetworkDto
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;

    public SocialNetwork ToValueObject()
        => SocialNetwork.Create(title: Title, url: Url).Value;
}