using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds
{
  public class AddressSeed
  {
    public const string VALID_STREET_FOR_STOREID1 = "Street 1";

    public static List<Address> Get()
    {
      var addresses = new List<Address>();

      for (int i = 1; i <= 100; i++)
      {
        addresses.Add(new Address()
        {
          Id = i,
          Street = $"Street {i}",
          StoreId = i
        });
      }

      return addresses;
    }
  }
}
