using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresOrderedSpecByName : Specification<Store>
    {
        public StoresOrderedSpecByName()
        {
            Query.OrderBy(x => x.Name);
        }
    }
}