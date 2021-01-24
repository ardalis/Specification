using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeNameSpec : Specification<Store>
    {
        public StoreIncludeNameSpec()
        {
            Query.Include(x => x.Name);
        }
    }
}
