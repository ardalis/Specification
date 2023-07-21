using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Specifications;

/// <summary>
/// This specification expects customer names to be unique - change the base type if you want to support multiple results
/// </summary>
public class CustomerByNameSpec : SingleResultSpecification<Customer>
{
  public CustomerByNameSpec(string name)
  {
    Query.Where(x => x.Name == name)
         .OrderBy(x => x.Name)
            .ThenByDescending(x => x.Address);
  }
}
