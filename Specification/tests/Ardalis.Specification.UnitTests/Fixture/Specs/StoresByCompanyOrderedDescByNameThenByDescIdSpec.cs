using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresByCompanyOrderedDescByNameThenByDescIdSpec : Specification<Store>
    {
        public StoresByCompanyOrderedDescByNameThenByDescIdSpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name)
                 .ThenByDescending(x => x.Id);
        }
    }
}