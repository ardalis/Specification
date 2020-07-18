using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
    {
        public StoreIncludeCompanyCountryDotSeparatedSpec()
        {
            Query.Include(x => x.Company.Country);
        }
    }
}