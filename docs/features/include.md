---
layout: default
title: Include
nav_order: 5
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# Include

The `Include` feature is used to indicate to the ORM that a related navigation property should be returned along with the base record being queried. It is used to expand the amount of related data being returned with an entity, providing [eager loading of related data](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager).

Compatible with the following providers:
- [EF Core](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager)
- [EF6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

**Note**: [*Lazy-loading* is not recommended in web-based .NET applications](https://ardalis.com/avoid-lazy-loading-entities-in-asp-net-applications/).

Below is a specification that loads a Company entity along with its collection of Stores.

```csharp
public class CompanySpec : Specification<Company>
{
    public CompanySpec(int id)
    {
        Query.Where(company => company.Id == id)
             .Include(x => x.Stores);
    }
}
```

# ThenInclude

The `ThenInclude` feature is used to indicate to the ORM that a related property of a previously `Include`d property should be returned with a query result.

Compatible with the following providers:
- [EF Core](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager)
- [EF6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

Below is a specification that loads a Company entity along with its collection of Stores, *then* each Store's collection of Products.

```csharp
public class CompanySpec : Specification<Company>
{
    public CompanySpec(int id)
    {
        Query.Where(company => company.Id == id)
             .Include(x => x.Stores)
             .ThenInclude(x => x.Products);
    }
}
```