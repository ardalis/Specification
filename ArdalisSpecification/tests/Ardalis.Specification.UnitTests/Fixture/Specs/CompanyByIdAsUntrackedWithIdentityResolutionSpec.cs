using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class CompanyByIdAsUntrackedWithIdentityResolutionSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdAsUntrackedWithIdentityResolutionSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTrackingWithIdentityResolution();
        }
    }
}
