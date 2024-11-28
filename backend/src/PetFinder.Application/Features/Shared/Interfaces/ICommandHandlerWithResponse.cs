using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared;

namespace PetFinder.Application.Features.Shared.Interfaces;

internal interface ICommandHandlerWithResponse<in T, TResponse> where T : ICommand
{
    Task<Result<TResponse, ErrorList>> Handle(T command, CancellationToken cancellationToken);
}

internal interface ICommandHandler<in T> where T : ICommand
{
    Task<UnitResult<ErrorList>> Handle(T command, CancellationToken cancellationToken);
}