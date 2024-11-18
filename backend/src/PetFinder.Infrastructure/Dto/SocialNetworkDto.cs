using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Infrastructure.Dto;

internal class SocialNetworkDto
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;

    public SocialNetwork ToValueObject()
        => SocialNetwork.Create(title: Title, url: Url).Value;
}