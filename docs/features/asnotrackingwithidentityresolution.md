---
layout: default
title: AsNoTrackingWithIdentityResolution
nav_order: 2
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# AsNoTrackingWithIdentityResolution

The `AsNoTrackingWithIdentityResolution` feature applies this method to the query executed by [EF Core](https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries).

Compatible with the following providers:
- [EF Core](https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries)
- EF6 - not supported

No-tracking queries can be forced to perform identity resolution by using `AsNoTrackingWithIdentityResolution`. The query will then keep track of returned instances (without tracking them in the normal way) and ensure no duplicates are created in the query results.

The following example shows how to add `AsNoTrackingWithIdentityResolution` to a specification.

```csharp
public class CustomerReadOnlySpec : Specification<Customer>
{
    public CustomerReadOnlySpec(string name)
    {
        Query.Where(x => x.Name == name)
             .AsNoTrackingWithIdentityResolution();
    }
}
```

🔍 Note: It's a good idea to note when specifications use `AsNoTracking` (or `AsNoTrackingWithIdentityResolution`) so that consumers of the specification will not attempt to modify and save entities returned by queries using the specification. The above specification adds `ReadOnly` to its name for this purpose.
