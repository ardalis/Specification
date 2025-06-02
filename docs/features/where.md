---
layout: default
title: Where
nav_order: 1
has_children: false
parent: Base Features
grand_parent: Features
---

# Where

The `Where` feature in specifications behaves the same as LINQ’s `Where` method. It accepts an `Expression<Func<TSource, bool>>` as a parameter and is used to define filtering criteria.

```csharp
 public class CustomerByLastNameSpec : Specification<Customer>
 {
     public CustomerByLastNameSpec(string lastName)
     {
         Query.Where(c => c.LastName == lastName);
     }
 }
```

The `Where` method also provides an overload that accepts a `bool condition` parameter. If the condition is false, the predicate is not added to the specification state. This is especially useful in dynamic scenarios similar to `WhereIf` extensions found in many LINQ helper libraries.

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
    }

    // Users can do this
    public CustomerSpec(CustomerFilter filter)
    {
        Query
            .Where(x => x.Name == filter.Name, !string.IsNullOrEmpty(filter.Name))
            .Where(x => x.Email == filter.Email, !string.IsNullOrEmpty(filter.Email));
    }
}
```
