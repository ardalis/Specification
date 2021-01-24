using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresPaginatedSpec : Specification<Store>
    {
        public StoresPaginatedSpec(int skip, int take)
        {
            Query.Paginate(skip, take);
        }
    }
}