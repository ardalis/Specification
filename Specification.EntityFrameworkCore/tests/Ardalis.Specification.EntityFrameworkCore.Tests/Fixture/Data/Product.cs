namespace Tests.Fixture;

public record Product
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public List<ProductImage>? Images { get; set; }
}
