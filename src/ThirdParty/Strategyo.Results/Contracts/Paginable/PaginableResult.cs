namespace Strategyo.Results.Contracts.Paginable;

public class PaginableResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasNextPage => PageNumber     < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}