namespace Tests.Builders;

public class Builder_Cache
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoEnableCache()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.CacheKey.Should().BeNull();
        spec1.CacheEnabled.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.CacheEnabled.Should().BeFalse();
    }

    [Fact]
    public void DoesNothing_GivenEnableCacheWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .EnableCache("asd", false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .EnableCache("asd", false);

        spec1.CacheKey.Should().BeNull();
        spec1.CacheEnabled.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.CacheEnabled.Should().BeFalse();
    }

    [Fact]
    public void ThrowsArgumentException_GivenNullSpecificationName()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        Action sut1 = () => spec1.Query.EnableCache(null!);
        Action sut2 = () => spec2.Query.EnableCache(null!);

        sut1.Should().Throw<ArgumentException>();
        sut2.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetsCacheKey_GivenEnableCache()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .EnableCache("asd", "x", "y");

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .EnableCache("asd", "x", "y");

        spec1.CacheKey.Should().Be("asd-x-y");
        spec1.CacheEnabled.Should().BeTrue();

        spec1.CacheKey.Should().Be("asd-x-y");
        spec2.CacheEnabled.Should().BeTrue();
    }

    [Fact]
    public void DoesNothing_GivenNoWithCacheKey()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.CacheKey.Should().BeNull();
        spec1.CacheEnabled.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.CacheEnabled.Should().BeFalse();
    }

    [Fact]
    public void DoesNothing_GivenWithCacheKeyWithFalseCondition()
    {
        var key = "someKey";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .WithCacheKey(key, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .WithCacheKey(key, false);

        spec1.CacheKey.Should().BeNull();
        spec1.CacheEnabled.Should().BeFalse();

        spec2.CacheKey.Should().BeNull();
        spec2.CacheEnabled.Should().BeFalse();
    }

    [Fact]
    public void SetsCacheKey_GivenWithCacheKey()
    {
        var key = "someKey";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .WithCacheKey(key);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .WithCacheKey(key);

        spec1.CacheKey.Should().Be(key);
        spec1.CacheEnabled.Should().BeTrue();

        spec1.CacheKey.Should().Be(key);
        spec2.CacheEnabled.Should().BeTrue();
    }

    [Fact]
    public void ThrowsArgumentException_GivenWithCacheKeyAndNullSpecificationName()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        Action sut1 = () => spec1.Query.WithCacheKey(null!);
        Action sut2 = () => spec2.Query.WithCacheKey(null!);

        sut1.Should().Throw<ArgumentException>();
        sut2.Should().Throw<ArgumentException>();
    }
}
