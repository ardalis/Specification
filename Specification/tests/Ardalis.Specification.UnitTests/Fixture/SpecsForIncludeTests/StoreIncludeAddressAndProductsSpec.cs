using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeAddressAndProductsSpec : Specification<Store>
    {
        public StoreIncludeAddressAndProductsSpec()
        {
            Query.Include(x => x.Products)
                 .Include(x=>x!.Address);
        }
    }
}
