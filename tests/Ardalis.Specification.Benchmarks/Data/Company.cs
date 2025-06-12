namespace Ardalis.Specification.Benchmarks;

public class Company
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Country Country { get; set; } = default!;
    public List<Store> Stores { get; set; } = [];
}
