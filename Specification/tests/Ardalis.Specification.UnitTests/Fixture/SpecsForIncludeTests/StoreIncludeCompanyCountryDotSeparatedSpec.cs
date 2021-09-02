using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
    {
        public StoreIncludeCompanyCountryDotSeparatedSpec()
        {
            Query.Include(x => x.Company!.Country);
        }
    }
}