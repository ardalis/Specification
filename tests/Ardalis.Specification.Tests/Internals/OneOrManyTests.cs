#if NET8_0_OR_GREATER

namespace Tests.Internals;

public class OneOrManyTests
{
    [Fact]
    public void IsEmpty_ReturnTrue_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        oneOrMany.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_ReturnFalse_GivenItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void HasSingleItem_ReturnFalse_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        oneOrMany.HasSingleItem.Should().BeFalse();
    }

    [Fact]
    public void HasSingleItem_ReturnTrue_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.HasSingleItem.Should().BeTrue();
    }

    [Fact]
    public void HasSingleItem_ReturnFalse_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "foo", "bar" };

        oneOrMany.HasSingleItem.Should().BeFalse();
    }

    [Fact]
    public void Add_CreatesSingleItem_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();
        oneOrMany.Add("foo");

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<string>();
        value.Should().Be("foo");
    }

    [Fact]
    public void Add_CreatesListWithTwoItems_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.Add("bar");

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "foo", "bar" });
    }

    [Fact]
    public void Add_AddsToTheList_GivenTwoItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "foo", "bar" };

        oneOrMany.Add("baz");

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "foo", "bar", "baz" });
    }

    [Fact]
    public void Add_DoesNothing_GivenInvalidState()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new string[] { "foo", "bar" };

        oneOrMany.Add("baz");

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<string[]>();
        value.Should().BeEquivalentTo(new string[] { "foo", "bar" });
    }

    [Fact]
    public void Single_ReturnsSingleItem_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.Single.Should().Be("foo");
    }

    [Fact]
    public void Single_Throws_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        var action = () => oneOrMany.Single;
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Single_Throws_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new string[] { "foo", "bar" };

        var action = () => _ = oneOrMany.Single;
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Values_ReturnsEmpty_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        oneOrMany.Values.Should().BeEmpty();
        oneOrMany.Values.Should().BeSameAs(Enumerable.Empty<string>());
    }

    [Fact]
    public void Values_ReturnsSingleItemEnumerable_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.Values.Should().ContainSingle();
        oneOrMany.Values.Should().BeEquivalentTo(new List<string> { "foo" });
    }

    [Fact]
    public void Values_ReturnsEnumerable_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "foo", "bar" };

        oneOrMany.Values.Should().BeEquivalentTo(new List<string> { "foo", "bar" });
    }

    [Fact]
    public void Values_Throws_GivenInvalidState()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new string[] { "foo", "bar" };

        var action = () => _ = oneOrMany.Values;
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Clone_ReturnEqualStruct_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        var clone = oneOrMany.Clone();

        clone.Values.Should().BeEquivalentTo(oneOrMany.Values);
    }

    [Fact]
    public void Clone_ReturnEqualStruct_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "foo", "bar" };

        var clone = oneOrMany.Clone();

        clone.Values.Should().BeEquivalentTo(oneOrMany.Values);
    }

    [Fact]
    public void Clone_ReturnsNewEmptyStruct_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        var clone = oneOrMany.Clone();

        clone.Values.Should().BeEquivalentTo(oneOrMany.Values);
    }

    private class Accessors
    {
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_value")]
        public static extern ref object? ValueOf(ref OneOrMany<string> @this);
    }
}

#endif
