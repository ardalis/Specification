using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreNamesPaginatedSpec : Specification<Store, string?>
    {
        public StoreNamesPaginatedSpec(int skip, int take)
        {
            Query.Skip(skip)
                 .Take(take);
            Query.Select(x => x.Name);
        }
    }
}