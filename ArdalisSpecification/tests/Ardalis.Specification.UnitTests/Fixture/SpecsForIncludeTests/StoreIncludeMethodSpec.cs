using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreIncludeMethodSpec : Specification<Store>
    {
        public StoreIncludeMethodSpec()
        {
            Query.Include(x => x.GetSomethingFromStore());
        }
    }
}