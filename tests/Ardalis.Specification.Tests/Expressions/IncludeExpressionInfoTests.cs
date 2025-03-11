namespace Tests.Expressions;

public class IncludeExpressionInfoTests
{
    public record Customer(int Id, Address Address);
    public record Address(int Id, City City);
    public record City(int Id);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForLambdaExpression()
    {
        var sut = () => new IncludeExpressionInfo(null!, IncludeTypeEnum.Include);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("expression");
    }

    [Fact]
    public void Constructor_GivenIncludeExpression()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        var sut = new IncludeExpressionInfo(expr, IncludeTypeEnum.Include);

        sut.Type.Should().Be(IncludeTypeEnum.Include);
        sut.LambdaExpression.Should().Be(expr);
    }
}
