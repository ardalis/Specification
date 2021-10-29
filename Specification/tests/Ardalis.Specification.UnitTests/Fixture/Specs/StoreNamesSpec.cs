using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreNamesSpec : Specification<Store, string?>
    {
        public StoreNamesSpec()
        {
            Query.Select(x => x.Name);
        }
    }
}