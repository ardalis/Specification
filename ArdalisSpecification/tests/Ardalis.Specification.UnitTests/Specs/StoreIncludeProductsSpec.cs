using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec()
        {
            Query.Include(x => x.Products);
        }
    }
}
