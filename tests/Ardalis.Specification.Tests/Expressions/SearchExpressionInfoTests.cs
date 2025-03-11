namespace Tests.Expressions;

public class SearchExpressionInfoTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    {
        var sut = () => new SearchExpressionInfo<Customer>(null!, "x");

        sut.Should().Throw<ArgumentNullException>().WithParameterName("selector");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullPattern()
    {
        Expression<Func<Customer, string?>> expr = x => x.Name;
        var sut = () => new SearchExpressionInfo<Customer>(expr, null!);

        sut.Should().Throw<ArgumentException>().WithMessage("The search term can not be null or empty.");
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void Constructor_GivenValidValues()
    {
        Expression<Func<Customer, string?>> expr = x => x.Name;

        var sut = new SearchExpressionInfo<Customer>(expr, "x", 99);

        sut.Selector.Should().Be(expr);
        sut.SearchTerm.Should().Be("x");
        sut.SearchGroup.Should().Be(99);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.SelectorFunc.Should().NotBeNull();
        //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_selectorFunc")]
        public static extern ref Func<T, string>? FuncFieldOf(SearchExpressionInfo<T> @this);
    }
#endif
}
