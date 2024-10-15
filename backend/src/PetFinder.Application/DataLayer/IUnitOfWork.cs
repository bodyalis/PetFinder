using System.Data;

namespace PetFinder.Application.DataLayer;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken);
    
    Task<int> SaveChanges(CancellationToken cancellationToken);
}