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
    public void AddSorted_CreatesSingleItem_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();
        oneOrMany.AddSorted("foo", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<string>();
        value.Should().Be("foo");
    }

    [Fact]
    public void AddSorted_InsertsInPosition_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.AddSorted("bar", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "bar", "foo" });
    }

    [Fact]
    public void AddSorted_AddsToTheEnd_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "bar";

        oneOrMany.AddSorted("foo", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "bar", "foo" });
    }

    [Fact]
    public void AddSorted_InsertsInPosition_GivenTwoItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "bar", "foo" };

        oneOrMany.AddSorted("baz", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "bar", "baz", "foo" });
    }

    [Fact]
    public void AddSorted_AddsToTheEnd_GivenTwoItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "bar", "baz" };

        oneOrMany.AddSorted("foo", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<List<string>>();
        value.Should().BeEquivalentTo(new List<string> { "bar", "baz", "foo" });
    }

    [Fact]
    public void AddSorted_ThrowsArgumentNullException_GivenNullComparer()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "bar", "baz" };

        var action = () => oneOrMany.AddSorted("foo", null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddSorted_DoesNothing_GivenInvalidState()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new string[] { "foo", "bar" };

        oneOrMany.AddSorted("baz", Comparer<string>.Default);

        var value = Accessors.ValueOf(ref oneOrMany);
        value.Should().BeOfType<string[]>();
        value.Should().BeEquivalentTo(new string[] { "foo", "bar" });
    }

    [Fact]
    public void List_ReturnsList_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new List<string> { "foo", "bar" };

        oneOrMany.List.Should().BeOfType<List<string>>();
        oneOrMany.List.Should().Equal(new List<string> { "foo", "bar" });
    }

    [Fact]
    public void List_Throws_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        var action = () => oneOrMany.List;
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void List_Throws_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        var action = () => _ = oneOrMany.List;
        action.Should().Throw<InvalidOperationException>();
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
    public void SingleOrDefault_ReturnsSingleItem_GivenSingleItem()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = "foo";

        oneOrMany.SingleOrDefault.Should().Be("foo");
    }

    [Fact]
    public void SingleOrDefault_ReturnsDefault_GivenEmptyStruct()
    {
        var oneOrMany = new OneOrMany<string>();

        oneOrMany.SingleOrDefault.Should().BeNull();
    }

    [Fact]
    public void SingleOrDefault_ReturnsDefault_GivenMultipleItems()
    {
        var oneOrMany = new OneOrMany<string>();
        Accessors.ValueOf(ref oneOrMany) = new string[] { "foo", "bar" };

        oneOrMany.SingleOrDefault.Should().BeNull();
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
