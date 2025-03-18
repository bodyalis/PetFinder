using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared;

namespace PetFinder.Application.Features.Shared.Interfaces;

public interface IQueryHandler<in T, TResponse>
    where T : IQuery
{
    Task<Result<TResponse, ErrorList>> Handle(T query, CancellationToken cancellationToken);
}