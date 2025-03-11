namespace Tests.Expressions;

public class WhereExpressionInfoTests
{
    public record Customer(int Id);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    {
        var sut = () => new WhereExpressionInfo<Customer>(null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("filter");
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void Constructor_GivenValidValues()
    {
        Expression<Func<Customer, bool>> expr = x => x.Id == 1;

        var sut = new WhereExpressionInfo<Customer>(expr);

        sut.Filter.Should().Be(expr);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.FilterFunc.Should().NotBeNull();
        //sut.FilterFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_filterFunc")]
        public static extern ref Func<T, bool>? FuncFieldOf(WhereExpressionInfo<T> @this);
    }
#endif
}
