namespace Tests.Fixture;

public record Country
{
    public int Id { get; set; }
    public int No { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
}
