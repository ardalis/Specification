namespace Tests.Fixture;

public class Bar
{
    public int Id { get; set; }
    public string? Dummy { get; set; }

    private readonly List<BarChild> _barChildren = [];
    public IReadOnlyCollection<BarChild> BarChildren => _barChildren.AsReadOnly();
}

public class BarChild
{
    public int Id { get; set; }
    public string? Dummy { get; set; }

    public int BarId { get; set; }
    public Bar Bar { get; set; } = default!;
}

public class BarDerived : BarChild
{
    public int BarDerivedInfoId { get; set; }
    public BarDerivedInfo BarDerivedInfo { get; set; } = default!;
}

public class BarDerivedInfo
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
