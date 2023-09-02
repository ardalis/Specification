using Xunit;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture.Collections;

[CollectionDefinition("WriteCollection")]
public class WriteCollection : ICollectionFixture<DatabaseFixture>
{
    public WriteCollection()
    {

    }
}
