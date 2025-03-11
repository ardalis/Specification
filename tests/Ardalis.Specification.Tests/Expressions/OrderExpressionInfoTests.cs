namespace Tests.Expressions;

public class OrderExpressionInfoTests
{
    public record Customer(int Id, string Name);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullExpression()
    {
        var sut = () => new OrderExpressionInfo<Customer>(null!, OrderTypeEnum.OrderBy);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("keySelector");
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void Constructor_GivenValidValues()
    {
        Expression<Func<Customer, object?>> expr = x => x.Name;

        var sut = new OrderExpressionInfo<Customer>(expr, OrderTypeEnum.OrderBy);

        sut.KeySelector.Should().Be(expr);
        sut.OrderType.Should().Be(OrderTypeEnum.OrderBy);
        Accessors<Customer>.FuncFieldOf(sut).Should().BeNull();
        sut.KeySelectorFunc.Should().NotBeNull();
        //sut.KeySelectorFunc.Should().BeEquivalentTo(expr.Compile());
        Accessors<Customer>.FuncFieldOf(sut).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_keySelectorFunc")]
        public static extern ref Func<T, object?>? FuncFieldOf(OrderExpressionInfo<T> @this);
    }
#endif
}
