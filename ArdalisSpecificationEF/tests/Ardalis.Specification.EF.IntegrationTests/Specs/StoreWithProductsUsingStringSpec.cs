using Ardalis.Specification.EF.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EF.IntegrationTests.Specs
{
    public class StoreWithProductsUsingStringSpec : Specification<Store>
    {
        public StoreWithProductsUsingStringSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(nameof(Store.Products));
        }
    }
}