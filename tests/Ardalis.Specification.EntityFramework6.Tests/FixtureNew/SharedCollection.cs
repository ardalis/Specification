using Xunit;

namespace Tests.FixtureNew;

[CollectionDefinition("SharedCollectionNew")]
public class SharedCollection : ICollectionFixture<TestFactory>
{
}
