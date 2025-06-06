---
layout: default
title: IgnoreAutoIncludes
nav_order: 4
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# IgnoreAutoIncludes

The `IgnoreAutoIncludes` feature is used to instruct `EF Core` to ignore automatic includes for this query. This is useful when you want to override the default behavior of automatically including navigation properties configured with `AutoInclude` in your model. It simply passes along this call to the underlying [EF Core feature for disabling automatic includes](https://learn.microsoft.com/ef/core/querying/related-data/auto-includes#ignoring-auto-includes).

Compatible with the following providers:
- [EF Core](https://learn.microsoft.com/ef/core/querying/related-data/auto-includes#ignoring-auto-includes)
- EF6 - not supported

The following example shows how to add `IgnoreAutoIncludes` to a specification.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name)
             .IgnoreAutoIncludes();
    }
}
```
