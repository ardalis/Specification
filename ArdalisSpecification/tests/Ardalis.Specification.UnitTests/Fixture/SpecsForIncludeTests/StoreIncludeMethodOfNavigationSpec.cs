using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
    {
        public StoreIncludeMethodOfNavigationSpec()
        {
            Query.Include(x => x.Address!.GetSomethingFromAddress());
        }
    }
}