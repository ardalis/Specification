using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyThenCountrySpec : Specification<Store>
    {
        public StoreIncludeCompanyThenCountrySpec()
        {
            Query.Include(x => x.Company)
                 .ThenInclude(x=>x!.Country);
        }
    }
}
