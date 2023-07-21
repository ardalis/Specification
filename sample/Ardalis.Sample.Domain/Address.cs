namespace Ardalis.Sample.Domain;

public class Address
{
    public int Id { get; set; }
    public required string Street { get; set; }

    public int CustomerId { get; set; }
}
