namespace Tests.Expressions;

public class IncludeExpressionInfoTests
{
    public record Customer(int Id, Address Address);
    public record Address(int Id, City City);
    public record City(int Id);

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForLambdaExpression()
    {
        var sut = () => new IncludeExpressionInfo(null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("expression");


        sut = () => new IncludeExpressionInfo(null!, typeof(Customer));

        sut.Should().Throw<ArgumentNullException>().WithParameterName("expression");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        var sut = () => new IncludeExpressionInfo(expr, null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("previousPropertyType");
    }

    [Fact]
    public void Constructor_GivenIncludeExpression()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;
        var sut = new IncludeExpressionInfo(expr);

        sut.Type.Should().Be(IncludeTypeEnum.Include);
        sut.LambdaExpression.Should().Be(expr);
    }

    [Fact]
    public void Constructor_GivenThenIncludeExpressionAndPreviousPropertyType()
    {
        Expression<Func<Address, City>> expr = x => x.City;
        var previousPropertyType = typeof(Customer);
        var sut = new IncludeExpressionInfo(expr, previousPropertyType);

        sut.Type.Should().Be(IncludeTypeEnum.ThenInclude);
        sut.LambdaExpression.Should().Be(expr);
        sut.PreviousPropertyType.Should().Be(previousPropertyType);
    }
}
