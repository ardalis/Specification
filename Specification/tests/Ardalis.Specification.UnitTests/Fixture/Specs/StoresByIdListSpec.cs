using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresByIdListSpec : Specification<Store>
    {
        public StoresByIdListSpec(IEnumerable<int> Ids)
        {
            Query.Where(x => Ids.Contains(x.Id));
        }
    }
}
