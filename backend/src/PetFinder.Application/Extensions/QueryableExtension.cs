using Microsoft.EntityFrameworkCore;
using PetFinder.Application.Models;

namespace PetFinder.Application.Extensions;

public static class QueryableExtension
{
    public static async Task<PagedList<T>> GetWithPagination<T>(this IQueryable<T> set,
        int page,
        int pageSize)
    {
        var totalCount = set.CountAsync();
        
        var items = await set
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<T>()
        {
            PageSize = pageSize,
            Page = page,
            TotalCount = await totalCount,
            Items = items,
        };

    }
}