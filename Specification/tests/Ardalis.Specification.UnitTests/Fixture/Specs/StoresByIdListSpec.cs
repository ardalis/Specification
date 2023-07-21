using System;
using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class StoresByIdListSpec : Specification<Store>
{
    public StoresByIdListSpec(IEnumerable<int> ids)
    {
        Query.Where(x => ids.Contains(x.Id));
    }
}
