using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreWithInMemorySpec : Specification<Store>
    {
        public StoreWithInMemorySpec()
        {
            Query.InMemory(x => x);
        }
    }
}