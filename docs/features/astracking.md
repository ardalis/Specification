---
layout: default
title: AsTracking
nav_order: 8
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# AsTracking

The `AsTracking` feature applies this method to the query executed by [EF Core](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.astracking). By default, all queries in EF Core are tracked. However, if you have configured the DbContext to use `NoTracking` as the default tracking behavior (for example, via `ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking`), then `AsTracking` becomes useful for enabling tracking on a per-query basis whenever you actually need to track a specific query.

Compatible with the following providers:
- [EF Core](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.astracking)
- EF6 - not supported

The following example shows how to add `AsTracking` to a specification.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name)
             .AsTracking();
    }
}
```

🔍 Note: Use `AsTracking` when you need to update entities. For read-only scenarios, prefer `AsNoTracking` for better performance.
