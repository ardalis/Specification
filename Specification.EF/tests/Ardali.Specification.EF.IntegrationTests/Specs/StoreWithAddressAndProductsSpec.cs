using Ardalis.Specification.EF.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EF.IntegrationTests.Specs
{
    public class StoreWithAddressAndProductsSpec : Specification<Store>
    {
        public StoreWithAddressAndProductsSpec(int id)
        {
            Query.Where(x => x.Id == id);
            Query.Include(x => x.Address);
            Query.Include(x => x.Products);
        }
    }
}
