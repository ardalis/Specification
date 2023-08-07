using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Collections;

[CollectionDefinition("ReadCollection")]
public class ReadCollection : ICollectionFixture<DatabaseFixture>
{
    public ReadCollection()
    {

    }
}
