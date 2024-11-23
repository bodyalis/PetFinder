using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared;

namespace PetFinder.Application.Features.Shared.Interfaces;

internal interface IQueryHandler<in T, TResponse> : IHandler
    where T : IQuery
{
    Task<Result<TResponse, ErrorList>> Handle(T query, CancellationToken cancellationToken);
}