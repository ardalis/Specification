using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds
{
  public class CompanySeed
  {
    public const int VALID_COMPANY_ID = 1;
    public const string VALID_COMPANY_NAME = "Company 1";

    public static List<Company> Get()
    {
      var companies = new List<Company>();

      companies.Add(new Company()
      {
        Id = 1,
        Name = "Company 1",
        CountryId = 1,
      });

      companies.Add(new Company()
      {
        Id = 2,
        Name = "Company 2",
        CountryId = 2,
      });

      companies.Add(new Company()
      {
        Id = 3,
        Name = "Company 3",
        CountryId = 1,
      });

      return companies;
    }
  }
}
