using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Specifications
{
    public class CustomerByNameWithStoresSpec : Specification<Customer>, ISingleResultSpecification
    {
        public CustomerByNameWithStoresSpec(string name)
        {
            Query.Where(x => x.Name == name)
                .Include(x => x.Stores)
                .EnableCache(nameof(CustomerByNameWithStoresSpec), name);
        }
    }
}
