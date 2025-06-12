namespace Ardalis.Specification.Benchmarks;

public class Country
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Company> Companies { get; set; } = [];
}
