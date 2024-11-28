namespace PetFinder.Application.Models;

public class PagedList<T>
{
    public const int MinPageNumber = 1;
    public IReadOnlyList<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage => Items.Count * PageSize < TotalCount;
    public bool HasPrevPage => Page > MinPageNumber;
}