using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFinder.Application.DataLayer;
using PetFinder.Infrastructure.DbContexts;

namespace PetFinder.Infrastructure;

public class UnitOfWork(WriteDbContext dbContext)
    : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken)
        => (await dbContext.Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}