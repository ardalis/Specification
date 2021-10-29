using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeProductsThenStoreSpec : Specification<Store>
    {
        public StoreIncludeProductsThenStoreSpec()
        {
            Query.Include(x => x.Products)
                 .ThenInclude(x=>x!.Store);
        }
    }
}
