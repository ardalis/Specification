---
layout: default
title: Frequently Asked Questions (FAQ)
parent: Getting Started
nav_order: 4
---

## The new version of .NET is out. Please support the new Target Framework.

The library sets only the minimum required TFM (Target Framework). If you're on a newer .NET version you still can consume the library with no issues. We do release new versions regularly, but not necessarily will follow the same cadence as .NET releases.

## Which versions of EF Core can I use?

The `Ardalis.Specification.EntityFrameworkCore` package depends only on the core `Microsoft.EntityFrameworkCore` package and sets the minimum required version. You can freely install a newer EF Core package explicitly. We do monitor the EF changes closely and will update our minimum required version as necessary. 

As a consumer, you will always install a EF provider package (e.g. `Microsoft.EntityFrameworkCore.SqlServer`). By doing so, the dependency graph is updated automatically and you'll be consuming the specified newer version, regardless of our defined minimum version. 

[![Dependencies in the csproj file showing Microsoft.EntityFrameworkCore.SqlServer version 9.0.2 and Ardalis.Specification.EntityFrameworkCore version 8.0.0. In the Packages list, there is also Microsoft.EntityFrameworkCore version 9.0.2 and Microsoft.EntityFrameworkCore.Relation version 9.0.2.](../assets/images/dependencies.png)](../assets/images/dependencies.png)


## How can I use the `Select` operator in Specification? The `Query` does not contain a `Select` method.

To use projections you must inherit from `Specification<T, TResult>` where `TResult` is the type you want to project into. This offers strongly typed experience in the builder and during the evaluation. Please refer to [the projection specification guidance](../usage/create-specifications.md#projection-specification) for more details.

## ISingleResultSpecification is obsolete?

I'm getting a warning or error telling me that `ISingleResultSpecification` is obsolete. What am I supposed to do to fix it?

Please use the generic version `ISingleResultSpecification<T>`. We also do provide a base `SingleResultSpecification<T>` class as a convenience. Please refer to [the single result specification guidance](../usage/create-specifications.md#single-result-specification) for more details.


## How to use OrderBy with string column name?

We have provided example implementations in the following [thread](https://github.com/ardalis/Specification/issues/53#issuecomment-776700662).

However, we strongly discourage these generic solutions. At first glance, it seems very appealing, but in practice, it's not. Let us list a few possible drawbacks:

- The property names in your API models and entities may not match. These models may evolve independently and in case of name mismatches you may end up with runtime exceptions.
- Ordering is an expensive operation (especially multi-level ordering), and you should have indexes for those columns in the database.
- The possibility for server-side ordering by all columns is not a feasible feature in practice. You'll have to define indexes for all columns. That's not a great DB design.
- Trying to implement this generically will involve a lot of reflection, which will additionally slow the execution.

A much more practical solution would be:
- Define the columns that support ordering, a few of them. This can be implemented as a dropdown in UI perhaps?
- The grid may additionally support ordering for all columns, but only client-side (the data that is already fetched).
- Create the mapping manually for the defined columns.

Here is a sample solution. Create an extension to the builder, so you can reuse the same logic in all your customer specifications.

```csharp
public static class CustomerSpecificationExtensions
{
  public static IOrderedSpecificationBuilder<Customer> ApplyOrdering(
    this ISpecificationBuilder<Customer> builder, 
    string sortBy, 
    string orderBy)
  {
    var isAscending = !orderBy?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

    return sortBy switch
    {
      "FirstName" => isAscending ? builder.OrderBy(x => x.FirstName) : builder.OrderByDescending(x => x.FirstName),
      "LastName" => isAscending ? builder.OrderBy(x => x.Surname) : builder.OrderByDescending(x => x.Surname),
      _ => builder.OrderBy(x => x.Id)
    };
  }
}

public class CustomerSpec : Specification<Customer>
{
  public CustomerSpec(string sortBy, string orderBy)
  {
    Query.ApplyOrdering(sortBy, orderBy);
  }
}
```