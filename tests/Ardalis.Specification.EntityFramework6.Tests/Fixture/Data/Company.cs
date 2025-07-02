namespace Tests.Fixture;

public record Company
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int CountryId { get; set; }
    public Country Country { get; set; } = default!;

    public List<Store> Stores { get; set; } = [];
}
