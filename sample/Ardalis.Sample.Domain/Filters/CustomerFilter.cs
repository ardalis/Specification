namespace Ardalis.Sample.Domain.Filters;

public record CustomerFilter : BaseFilter
{
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
    public string? Name { get; set; }
}
