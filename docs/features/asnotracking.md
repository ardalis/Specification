---
layout: default
title: AsNoTracking
nav_order: 6
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# AsNoTracking

The `AsNoTracking` feature applies this method to the query executed by [EF6](https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbextensions.asnotracking) or [EF Core](https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries).

Compatible with the following providers:
- [EF Core](https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries)
- [EF6](https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbextensions.asnotracking)

No-tracking queries are useful when the results are used in a read-only scenario. They're quicker to execute because there's no need to set up the change tracking information. If you don't need to update the entities retrieved from the database, then a no-tracking query should be used.

The following example shows how to add `AsNoTracking` to a specification.

```csharp
public class CustomerReadOnlySpec : Specification<Customer>
{
    public CustomerReadOnlySpec(string name)
    {
        Query.Where(x => x.Name == name)
             .AsNoTracking();
    }
}
```

🔍 Note: It's a good idea to note when specifications use `AsNoTracking` so that consumers of the specification will not attempt to modify and save entities returned by queries using the specification. The above specification adds `ReadOnly` to its name for this purpose.
