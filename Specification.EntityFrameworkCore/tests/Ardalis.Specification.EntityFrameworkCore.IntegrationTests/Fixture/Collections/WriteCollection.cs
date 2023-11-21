using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture.Collections;

[CollectionDefinition("WriteCollection")]
public class WriteCollection : ICollectionFixture<DatabaseFixture>
{
    public WriteCollection()
    {

    }
}
