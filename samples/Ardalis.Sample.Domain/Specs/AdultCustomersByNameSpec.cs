using Ardalis.Specification;

namespace Ardalis.Sample.Domain.Specs;

public class AdultCustomersByNameSpec : Specification<Customer>
{
    public AdultCustomersByNameSpec(string nameSubstring)
    {
        Query.IsAdult()
            .NameIncludes(nameSubstring);
    }
}
