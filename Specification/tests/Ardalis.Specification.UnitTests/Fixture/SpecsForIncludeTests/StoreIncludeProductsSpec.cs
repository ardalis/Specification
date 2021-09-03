using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec()
        {
            Query.Include(x => x.Products);
        }
    }
}
