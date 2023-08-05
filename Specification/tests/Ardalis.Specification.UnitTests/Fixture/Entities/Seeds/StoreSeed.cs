using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;

public class StoreSeed
{
#pragma warning disable IDE1006 // Naming Styles
    public const int VALID_STORE_ID = 1;
    public const string VALID_STORE_NAME = "Store 1";
    public const string VALID_STORE_City = "City 1";

    public const int VALID_Search_ID = 50;
    public const string VALID_Search_City_Key = "BCD";
    public const string VALID_Search_Name_Key = "BCE";
    public const string VALID_Search_City_Name_Key = "BC";


    public const int ORDERED_BY_NAME_FIRST_ID = 48;
    public const int ORDERED_BY_NAME_LAST_ID = 49;
    public const int ORDERED_BY_NAME_DESC_FIRST_ID = 49;
    public const int ORDERED_BY_NAME_DESC_LAST_ID = 48;

    public const int ORDERED_BY_NAME_FOR_COMPANY2_FIRST_ID = 98;
    public const int ORDERED_BY_NAME_FOR_COMPANY2_LAST_ID = 99;
    public const int ORDERED_BY_NAME_DESC_FOR_COMPANY2_FIRST_ID = 99;
    public const int ORDERED_BY_NAME_DESC_FOR_COMPANY2_LAST_ID = 98;
    public const int ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_FIRST_ID = 89;
    public const int ORDERED_BY_NAME_DESC_FOR_COMPANY2_PAGE2_LAST_ID = 80;
#pragma warning restore IDE1006 // Naming Styles

    public static List<Store> Get()
    {
        var stores = new List<Store>();

        for (var i = 1; i <= 50; i++)
        {
            stores.Add(new()
            {
                Id = i,
                Name = $"Store {i}",
                City = $"City {i}",
                AddressId = i,
                CompanyId = 1,
            });
        }
        for (var i = 51; i <= 100; i++)
        {
            stores.Add(new()
            {
                Id = i,
                Name = $"Store {i}",
                City = $"City {i}",
                AddressId = i,
                CompanyId = 2,
            });
        }

        stores[49 - 1].Name = "ZZZ";
        stores[48 - 1].Name = "AAA";
        stores[99 - 1].Name = "YYY";
        stores[98 - 1].Name = "BBB";

        stores[100 - 1].Name = "Store 999";

        stores[50 - 1].City = "ABCDEFGH";
        stores[50 - 1].Name = "ABCEFGH";

        return stores;
    }
}
