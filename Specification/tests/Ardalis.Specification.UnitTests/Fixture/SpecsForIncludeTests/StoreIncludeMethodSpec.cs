using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeMethodSpec : Specification<Store>
  {
    public StoreIncludeMethodSpec()
    {
      Query.Include(x => x.GetSomethingFromStore());
    }
  }
}
