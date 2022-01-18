using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeProductsSpec : Specification<Store>
  {
    public StoreIncludeProductsSpec()
    {
      Query.Include(x => x.Products);
    }
  }
}
