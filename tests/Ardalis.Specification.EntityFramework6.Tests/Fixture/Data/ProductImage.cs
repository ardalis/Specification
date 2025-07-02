namespace Tests.Fixture;

public record ProductImage
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
