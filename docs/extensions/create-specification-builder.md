---
layout: default
title: How to create your own specification builder
parent: Extensions
nav_order: 2
---


# How to create your own specification builder
How to create your own specification builder 

## Example: Configure caching behaviour through specification builder extension method

In order to achieve this:

````csharp
public class CustomerByNameWithStores : Specification<Customer>
{
    public CustomerByNameWithStores(string name)
    {
        Query.Where(x => x.Name == name)
            .EnableCache(nameof(CustomerByNameWithStoresSpec), name)
                // Can only be called after .EnableCache()
                .WithTimeToLive(TimeSpan.FromHours(1))
            .Include(x => x.Stores);
    }
}
````

We can create a simple extension method on the specification builder:

````csharp
public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> WithTimeToLive<T>(this ISpecificationBuilder<T> @this, TimeSpan ttl)
        where T : class
    {
        @this.Specification.SetCacheTTL(ttl);
        return @this;
    }
}
````

This extension method can only be called when chained after `SpecificationBuilderExtensions.EnableCache`. This is because `EnableCache` returns `ICacheSpecificationBuilder<T>` which inherits from `ISpecificationBuilder<T>`.

```csharp
// TODO: Repository example
```

Finally, we need to take of some plumbing to implement both `` and ``. The class below uses `ConditionalWeakTable` to do the trick. An other solution is to create a base class that inherits from `Specification<T>`.

````csharp
public static class SpecificationExtentions
{
    private static readonly ConditionalWeakTable<object, CacheOptions> SpecificationCacheOptions = new();

    public static void SetCacheTTL<T>(this ISpecification<T> spec, TimeSpan ttl)
    {
        SpecificationCacheOptions.AddOrUpdate(spec, new CacheOptions() { TTL = ttl });
    }

    public static TimeSpan GetCacheTTL<T>(this ISpecification<T> spec)
    {
        var opts = SpecificationCacheOptions.GetOrCreateValue(spec);
        return opts?.TTL ?? TimeSpan.MaxValue;
    }

    // ConditionalWeakTable need reference types; TimeSpan is a struct
    private class CacheOptions
    {
        public TimeSpan TTL { get; set; }
    }
}
````