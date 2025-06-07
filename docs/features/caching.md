---
layout: default
title: Caching
nav_order: 7
has_children: false
parent: Base Features
grand_parent: Features
---

# Caching

The `EnableCache` feature allows you to enable caching for a specification. When enabled, the results of the query can be cached by a compatible repository or infrastructure, improving performance for repeated queries with the same parameters.

## How to Enable Caching

Use the `EnableCache()` method in your specification. It accepts two parameters:
1. A cache key (typically the specification name)
2. Any additional parameters that uniquely identify the query

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name)
             .EnableCache(nameof(CustomerSpec), name);
    }
}
```

Alternatively, you can use the `WithCacheKey()` method to manually set a custom cache key.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name)
             .WithCacheKey($"{nameof(CustomerSpec)} - {name}");
    }
}
```

## How Caching Works

- The `EnableCache` and `WithCacheKey` methods store the cache key in the specification's internal state.
- The actual caching logic is implemented by the consuming infrastructure (e.g., a `CachedRepository`).

🔍 Note: `EnableCache` and `WithCacheKey` do not manage cache expiration or eviction. For advanced scenarios such as expiration policies or tagging, consider extending the builder. See [how to write specification extensions](../extensions/extensions-for-specifications.md) for examples.

## When to Use Caching

- ✅ Use caching for frequently repeated or expensive queries with stable input parameters.
- ❌ Avoid caching queries that return highly dynamic or user-specific data unless you implement proper cache invalidation strategies.
