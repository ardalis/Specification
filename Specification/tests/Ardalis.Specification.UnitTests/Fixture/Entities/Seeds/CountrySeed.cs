using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;

public class CountrySeed
{
  public static List<Country> Get()
  {
    var countries = new List<Country>();

    countries.Add(new Country()
    {
      Id = 1,
      Name = "Country 1",
    });

    countries.Add(new Country()
    {
      Id = 2,
      Name = "Country 2",
    });

    return countries;
  }
}
