using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Specifications
{
  public class CustomerByNameSpec : Specification<Customer>, ISingleResultSpecification
  {
    public CustomerByNameSpec(string name)
    {
      Query.Where(x => x.Name == name)
           .OrderBy(x => x.Name)
              .ThenByDescending(x => x.Address);
    }
  }
}
