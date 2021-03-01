using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

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
