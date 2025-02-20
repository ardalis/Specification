namespace Ardalis.Sample.Domain.Specs;

public static class CustomerPredicates
{
    public static Func<Customer, bool> IsAdult => customer => customer.Age >= 18;
    public static Func<Customer, int, bool> IsAtLeastYearsOld => (customer, years) => customer.Age >= years;
    public static Func<Customer, string, bool> NameIncludes => 
        (customer, nameString) => customer.Name.Contains(nameString, StringComparison.CurrentCultureIgnoreCase);
}
