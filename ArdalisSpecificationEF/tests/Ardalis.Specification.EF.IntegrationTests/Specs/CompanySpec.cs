using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Specs
{
    public class CompanySpec : Specification<Company>
    {
        public CompanySpec(int id)
        {
            Query.Where(company => company.Id == id);
        }
    }
}
