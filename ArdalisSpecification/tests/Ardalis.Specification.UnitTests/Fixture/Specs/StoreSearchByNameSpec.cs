using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreSearchByNameSpec : Specification<Store>
    {
        public StoreSearchByNameSpec(string searchTerm)
        {
            Query.Search(x => x.Name!, "%" + searchTerm + "%");
        }
    }
}
