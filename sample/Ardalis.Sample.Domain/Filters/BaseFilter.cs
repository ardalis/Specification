namespace Ardalis.Sample.Domain.Filters;

public record BaseFilter
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? OrderBy { get; set; }
}
