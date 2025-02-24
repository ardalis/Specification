namespace Tests.Builders;

public class Builder_ThenInclude
{
    public record Customer(int Id, Address Address, List<Address> Addresses);
    public record Address(int Id, Contact Contact, List<Contact> Contacts);
    public record Contact(int Id, Phone Phone, List<Phone> Phones);
    public record Phone(int Id, string Number);

    [Fact]
    public void DoesNothing_GivenIncludeThenWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact, false)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts, false)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact, false)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact, false)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts, false)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact, false)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts, false);

        spec1.IncludeExpressions.Should().HaveCount(4);
        spec1.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
        spec2.IncludeExpressions.Should().HaveCount(4);
        spec2.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
    }

    [Fact]
    public void DoesNothing_GivenIncludeThenWithDiscardedTopChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address, false)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address, false)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses, false)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses, false)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address, false)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address, false)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses, false)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses, false)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone);

        spec1.IncludeExpressions.Should().BeEmpty();
        spec2.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenIncludeThenWithDiscardedNestedChain()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts, false)
                .ThenInclude(x => x.Phone);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact, false)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts, false)
                .ThenInclude(x => x.Phone);

        spec1.IncludeExpressions.Should().HaveCount(4);
        spec1.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
        spec2.IncludeExpressions.Should().HaveCount(4);
        spec2.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
    }

    [Fact]
    public void AddsIncludeThen_GivenIncludeThen()
    {
        Expression<Func<Address, Contact>> expr = x => x.Contact;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address)
            .ThenInclude(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address)
            .ThenInclude(expr);

        spec1.IncludeExpressions.Should().HaveCount(2);
        spec1.IncludeExpressions.Last().LambdaExpression.Should().BeSameAs(expr);
        spec1.IncludeExpressions.First().Type.Should().Be(IncludeTypeEnum.Include);
        spec1.IncludeExpressions.Last().Type.Should().Be(IncludeTypeEnum.ThenInclude);
        spec2.IncludeExpressions.Should().HaveCount(2);
        spec2.IncludeExpressions.Last().LambdaExpression.Should().BeSameAs(expr);
        spec2.IncludeExpressions.First().Type.Should().Be(IncludeTypeEnum.Include);
        spec2.IncludeExpressions.Last().Type.Should().Be(IncludeTypeEnum.ThenInclude);
    }

    [Fact]
    public void AddsIncludeThen_GivenMultipleIncludeThen()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Address)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Phone)
            .Include(x => x.Addresses)
                .ThenInclude(x => x.Contacts)
                .ThenInclude(x => x.Phone);

        spec1.IncludeExpressions.Should().HaveCount(12);
        spec1.IncludeExpressions.OrderBy(x => x.Type).Take(4).Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
        spec1.IncludeExpressions.OrderBy(x => x.Type).Skip(4).Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.ThenInclude));
        spec2.IncludeExpressions.Should().HaveCount(12);
        spec2.IncludeExpressions.OrderBy(x => x.Type).Take(4).Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
        spec2.IncludeExpressions.OrderBy(x => x.Type).Skip(4).Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.ThenInclude));
    }
}
