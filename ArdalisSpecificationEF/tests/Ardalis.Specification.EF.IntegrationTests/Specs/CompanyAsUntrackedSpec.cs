using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class CompanyAsUntrackedSpec : Specification<Company>
    {
        public CompanyAsUntrackedSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTracking();
        }
    }
}
