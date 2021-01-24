using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreByIdIncludeProductsUsingStringSpec : Specification<Store>
    {
        public StoreByIdIncludeProductsUsingStringSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(nameof(Store.Products));
        }
    }
}