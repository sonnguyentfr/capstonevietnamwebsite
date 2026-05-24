namespace NVCMS.WebView.Data.Common;

public class PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrev => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PaginatedList(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        Items      = items.ToList().AsReadOnly();
        TotalCount = totalCount;
        Page       = page;
        PageSize   = pageSize;
    }
}