using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;

public class AddressSeed
{
#pragma warning disable IDE1006 // Naming Styles
    public const string VALID_STREET_FOR_STOREID1 = "Street 1";
#pragma warning restore IDE1006 // Naming Styles

    public static List<Address> Get()
    {
        var addresses = new List<Address>();

        for (var i = 1; i <= 100; i++)
        {
            addresses.Add(new()
            {
                Id = i,
                Street = $"Street {i}",
                StoreId = i
            });
        }

        return addresses;
    }
}
