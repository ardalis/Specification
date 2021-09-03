using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class CompanyIncludeFilteredStoresSpec : Specification<Company>
    {
        public CompanyIncludeFilteredStoresSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores.Where(s => s.Id == 1));
        }
    }
}