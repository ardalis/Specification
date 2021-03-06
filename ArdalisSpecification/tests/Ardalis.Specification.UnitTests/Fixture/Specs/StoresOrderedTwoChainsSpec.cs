using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresOrderedTwoChainsSpec : Specification<Store>
    {
        public StoresOrderedTwoChainsSpec()
        {
            Query.OrderBy(x => x.Name)
                .OrderBy(x => x.Id);
        }
    }
}