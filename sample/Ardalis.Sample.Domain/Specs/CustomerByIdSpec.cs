using Ardalis.Specification;

namespace Ardalis.Sample.Domain.Specs;

public class CustomerByIdSpec : SingleResultSpecification<Customer>
{
    public CustomerByIdSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Addresses);
    }
}
