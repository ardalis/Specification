using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreWithFaultyIncludeSpec : Specification<Store>
    {
        public StoreWithFaultyIncludeSpec()
        {
            Query.Include(x => x.Id == 1 && x.Name == "Something");
        }
    }
}