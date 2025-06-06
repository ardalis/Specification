using Ardalis.Specification;

namespace Ardalis.Sample.Domain.Specs;

public sealed class GetCustomers<TProj> : Specification<Customer, TProj>
{
    public GetCustomers(Func<IQueryable<Customer>, IQueryable<TProj>> selector)
    {
        Query.Select(selector);
    }
}
