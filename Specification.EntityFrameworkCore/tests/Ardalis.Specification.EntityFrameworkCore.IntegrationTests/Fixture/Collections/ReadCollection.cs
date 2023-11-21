using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture.Collections;

[CollectionDefinition("ReadCollection")]
public class ReadCollection : ICollectionFixture<DatabaseFixture>
{
    public ReadCollection()
    {

    }
}
