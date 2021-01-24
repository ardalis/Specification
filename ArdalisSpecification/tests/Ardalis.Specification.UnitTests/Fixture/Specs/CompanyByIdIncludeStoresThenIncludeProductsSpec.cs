using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class CompanyByIdIncludeStoresThenIncludeProductsSpec : Specification<Company>
    {
        public CompanyByIdIncludeStoresThenIncludeProductsSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Products);
        }
    }
}
