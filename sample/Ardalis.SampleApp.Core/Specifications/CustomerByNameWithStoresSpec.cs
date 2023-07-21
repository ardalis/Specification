using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Specifications;

/// <summary>
/// This specification expects customer names to be unique - change the base type if you want to support multiple results
/// </summary>
public class CustomerByNameWithStoresSpec : SingleResultSpecification<Customer>
{
  public CustomerByNameWithStoresSpec(string name)
  {
    Query.Where(x => x.Name == name)
        .Include(x => x.Stores)
        .EnableCache(nameof(CustomerByNameWithStoresSpec), name);
  }
}
