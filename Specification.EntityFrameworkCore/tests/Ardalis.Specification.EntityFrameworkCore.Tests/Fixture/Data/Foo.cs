namespace Tests.Fixture;

public class Foo
{
    public int Id { get; set; }
    public string? Dummy { get; set; }
    public OuterNavigation OuterNavigation { get; set; } = default!;

    public List<InnerNavigation> ListNavigation => _listNavigation;
    private readonly List<InnerNavigation> _listNavigation = [];
    public IEnumerable<InnerNavigation> IEnumerableNavigation => _iEnumerableNavigation.AsEnumerable();
    private readonly List<InnerNavigation> _iEnumerableNavigation = [];

    public IReadOnlyCollection<InnerNavigation> IReadOnlyCollectionNavigation => _iReadOnlyCollectionNavigation.AsReadOnly();
    private readonly List<InnerNavigation> _iReadOnlyCollectionNavigation = [];

    public IReadOnlyList<InnerNavigation> IReadOnlyListNavigation => _iReadOnlyListNavigation.AsReadOnly();
    private readonly List<InnerNavigation> _iReadOnlyListNavigation = [];
}

public class OuterNavigation
{
    public int Id { get; set; }
    public string? Dummy { get; set; }

    public InnerNavigation InnerNavigation { get; set; } = default!;
    public List<InnerNavigation> ListNavigation => _listNavigation;
    private readonly List<InnerNavigation> _listNavigation = [];
    public IEnumerable<InnerNavigation> IEnumerableNavigation => _iEnumerableNavigation.AsEnumerable();
    private readonly List<InnerNavigation> _iEnumerableNavigation = [];

    public IReadOnlyCollection<InnerNavigation> IReadOnlyCollectionNavigation => _iReadOnlyCollectionNavigation.AsReadOnly();
    private readonly List<InnerNavigation> _iReadOnlyCollectionNavigation = [];

    public IReadOnlyList<InnerNavigation> IReadOnlyListNavigation => _iReadOnlyListNavigation.AsReadOnly();
    private readonly List<InnerNavigation> _iReadOnlyListNavigation = [];
}

public class InnerNavigation
{
    public int Id { get; set; }
    public string? Dummy { get; set; }
    public InnerNavigation2 InnerNavigation2 { get; set; } = default!;
    public List<InnerNavigation2> ListNavigation2 { get; set; } = [];
}

public class InnerNavigation2
{
    public int Id { get; set; }
    public string? Dummy { get; set; }
}
