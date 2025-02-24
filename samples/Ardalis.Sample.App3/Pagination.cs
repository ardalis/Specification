using Ardalis.Sample.Domain.Filters;
using System.Text.Json.Serialization;

namespace Ardalis.Sample.App3;

public class Pagination
{
    private readonly PaginationSettings _paginationSettings;

    public int TotalItems { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int Page { get; }
    public int StartItem { get; }
    public int EndItem { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }

    [JsonIgnore]
    public int Take { get; }
    [JsonIgnore]
    public int Skip { get; }

    [JsonConstructor]
    public Pagination(int totalItems, int totalPages, int pageSize, int page, int startItem, int endItem, bool hasPrevious, bool hasNext)
    {
        _paginationSettings = default!;
        TotalItems = totalItems;
        TotalPages = totalPages;
        PageSize = pageSize;
        Page = page;
        StartItem = startItem;
        EndItem = endItem;
        HasPrevious = hasPrevious;
        HasNext = hasNext;
    }

    public Pagination(int itemsCount, BaseFilter baseFilter)
        : this(new PaginationSettings(10, 100), itemsCount, baseFilter.PageSize, baseFilter.Page)
    {
    }

    public Pagination(PaginationSettings paginationSettings, int itemsCount, BaseFilter baseFilter)
        : this(paginationSettings, itemsCount, baseFilter.PageSize, baseFilter.Page)
    {
    }

    public Pagination(PaginationSettings paginationSettings, int itemsCount, int? pageSize, int? page)
    {
        _paginationSettings = paginationSettings;

        // The order of actions is important
        TotalItems = GetHandledTotalItems(itemsCount);
        PageSize = GetHandledPageSize(pageSize);
        TotalPages = GetHandledTotalPages();
        Page = GetHandledPage(page);

        HasNext = Page != TotalPages;
        HasPrevious = Page != 1;

        StartItem = TotalItems == 0 ? 0 : (PageSize * (Page - 1)) + 1;
        EndItem = (PageSize * Page) > TotalItems ? TotalItems : PageSize * Page;

        Take = PageSize;
        Skip = PageSize * (Page - 1);
    }

    private int GetHandledTotalItems(int itemsCount)
    {
        return itemsCount < 0 ? 0 : itemsCount;
    }

    private int GetHandledPageSize(int? pageSize)
    {
        if (!pageSize.HasValue || pageSize <= 0) return _paginationSettings.DefaultPageSize;

        if (pageSize > _paginationSettings.DefaultPageSizeLimit) return _paginationSettings.DefaultPageSizeLimit;

        return pageSize.Value;
    }

    private int GetHandledTotalPages()
    {
        return TotalItems == 0 ? 1 : (int)Math.Ceiling((decimal)TotalItems / GetHandledPageSize(PageSize));
    }

    private int GetHandledPage(int? page)
    {
        if (!page.HasValue || page <= 0) return _paginationSettings.DefaultPage;

        if (page.Value > TotalPages) return TotalPages;

        return page.Value;
    }
}
