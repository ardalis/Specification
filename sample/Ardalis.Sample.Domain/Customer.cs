namespace Ardalis.Sample.Domain;

public class Customer : IAggregateRoot
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }

    public List<Address> Addresses { get; set; } = new();
}
