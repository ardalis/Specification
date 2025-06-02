---
layout: default
title: Base Features
nav_order: 1
has_children: true
parent: Features
---

# Base Features

The features described below are supported by all provider packages, including the in-memory evaluator.

All features are implemented as extension methods to `ISpecificationBuilder`, and they closely mimic LINQ syntax. These methods follow the same naming conventions and exhibit behavior consistent with their LINQ counterparts. For further clarification or deeper understanding of similar constructs, refer to the [LINQ documentation](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=net-5.0)

### Conditional Overloads

All builder extension methods offer an overload that accepts a `bool condition` parameter. When the provided condition is false, the expression or value is not added to the specification state. This is especially useful in dynamic scenarios where a condition may or may not apply, functionally similar to `WhereIf` style extensions often found in LINQ helper libraries.

```csharp
public class CustomerSpec : Specification<Customer>
{
    // Instead of having this
    public CustomerSpec(CustomerFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(x => x.Name == filter.Name);

        if (!string.IsNullOrEmpty(filter.Email))
            Query.Where(x => x.Email == filter.Email);

        if (!string.IsNullOrEmpty(filter.Address))
            Query.Search(x => x.Address, "%" + filter.Address + "%");
    }

    // Users can do this
    public CustomerSpec(CustomerFilter filter)
    {
        Query
            .Where(x => x.Name == filter.Name, !string.IsNullOrEmpty(filter.Name))
            .Where(x => x.Email == filter.Email, !string.IsNullOrEmpty(filter.Email))
            .Search(x => x.Address, "%" + filter.Address + "%", !string.IsNullOrEmpty(filter.Address));
    }
}
```