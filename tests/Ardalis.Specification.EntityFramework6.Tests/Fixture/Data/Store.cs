namespace Tests.Fixture;

public record Store
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Address Address { get; set; } = default!;

    public List<Product> Products { get; set; } = [];
}
