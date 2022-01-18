using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;


namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoresByCompanyPaginatedOrderedDescByNameSpec : Specification<Store>
  {
    public StoresByCompanyPaginatedOrderedDescByNameSpec(int companyId, int skip, int take)
    {
      Query.Where(x => x.CompanyId == companyId)
           .Skip(skip)
           .Take(take)
           .OrderByDescending(x => x.Name);
    }
  }
}
