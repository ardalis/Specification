using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.FixtureNew;

public record Address
{
    public int Id { get; set; }
    public string? Street { get; set; }

    public int StoreId { get; set; }
    public Store Store { get; set; } = default!;
}
