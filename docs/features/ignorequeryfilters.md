---
layout: default
title: IgnoreQueryFilters
nav_order: 4
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# IgnoreQueryFilters

The `IgnoreQueryFilters` feature is used to indicate to `EF Core` that it should ignore global query filters for this query. It simply passes along this call to the underlying [EF Core feature for disabling global filters](https://docs.microsoft.com/ef/core/querying/filters#disabling-filters).

Compatible with the following providers:
- [EF Core](https://docs.microsoft.com/ef/core/querying/filters#disabling-filters)
- EF6 - not supported

The following example shows how to add `IgnoreQueryFilters` to a specification.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name)
             .IgnoreQueryFilters();
    }
}
```
