using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Specifications
{
    public class CustomerByNameSpec : Specification<Customer>
    {
        public CustomerByNameSpec(string name)
        {
            Query.Where(x => x.Name == name)
                 .OrderBy(x => x.Name)
                    .ThenByDescending(x => x.Address);
        }
    }
}
