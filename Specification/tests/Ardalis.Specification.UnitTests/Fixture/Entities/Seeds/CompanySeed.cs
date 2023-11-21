using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;

public class CompanySeed
{
#pragma warning disable IDE1006 // Naming Styles
    public const int VALID_COMPANY_ID = 1;
    public const string VALID_COMPANY_NAME = "Company 1";
#pragma warning restore IDE1006 // Naming Styles

    public static List<Company> Get()
    {
        var companies = new List<Company>
        {
            new()
            {
                Id = 1,
                Name = "Company 1",
                CountryId = 1,
            },
            new()
            {
                Id = 2,
                Name = "Company 2",
                CountryId = 2,
            },
            new()
            {
                Id = 3,
                Name = "Company 3",
                CountryId = 1,
            }
        };

        return companies;
    }
}
