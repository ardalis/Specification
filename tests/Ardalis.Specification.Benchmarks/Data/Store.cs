namespace Ardalis.Specification.Benchmarks;

public class Store
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Company Company { get; set; } = default!;
    public List<Product> Products { get; set; } = [];
}
