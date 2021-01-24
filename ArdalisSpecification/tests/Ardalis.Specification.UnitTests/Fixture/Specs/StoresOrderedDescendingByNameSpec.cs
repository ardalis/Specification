using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresOrderedDescendingByNameSpec : Specification<Store>
    {
        public StoresOrderedDescendingByNameSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}