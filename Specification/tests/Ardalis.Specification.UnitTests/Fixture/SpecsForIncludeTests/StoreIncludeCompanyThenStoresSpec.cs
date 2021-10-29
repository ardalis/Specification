using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyThenStoresSpec : Specification<Store>
    {
        public StoreIncludeCompanyThenStoresSpec()
        {
            Query.Include(x => x.Company)
                 .ThenInclude(x=>x!.Stores);
        }
    }
}
