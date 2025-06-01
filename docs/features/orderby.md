---
layout: default
title: OrderBy
nav_order: 2
has_children: false
parent: Base Features
grand_parent: Features
---

# OrderBy

The `OrderBy` feature in specifications functions just like LINQ’s `OrderBy` method. It accepts an `Expression<Func<TSource, TKey>>` and is used to define the primary sort order for the query results.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec()
    {
        Query.OrderBy(x => x.Name);
    }
}
```

To sort in descending order, use `OrderByDescending`.

```csharp
Query.OrderByDescending(x => x.Name);
```

For multi-level sorting, use `ThenBy` and `ThenByDescending`.

```csharp
Query.OrderByDescending(x => x.Name)
     .ThenByDescending(x => x.Id)
     .ThenBy(x => x.DateCreated);
```

### Conditional Overloads

All ordering methods support an overload that accepts a `bool condition`. If the condition evaluates to false, the ordering expression is ignored. This is useful for building dynamic or optional sorting logic.

```csharp
public class CustomerSpec : Specification<Customer>
{
    // Instead of having this
    public CustomerSpec(bool shouldOrder)
    {
        if (shouldOrder)
        {
            Query.OrderBy(x => x.Name);
        }
    }

    // Users can do this
    public CustomerSpec(bool shouldOrder)
    {
        Query.OrderBy(x => x.Name, shouldOrder);
    }
}
```

Because ordering supports method chaining, conditional evaluation affects the entire chain. If a parent ordering method is not applied (due to a false condition), any chained `ThenBy` or `ThenByDescending` calls are automatically discarded.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec()
    {
        Query.OrderBy(x => x.Id, false)
             .ThenBy(x => x.Name); // since the parent is not added, this also will be discarded
    }
}

public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec()
    {
        Query.OrderBy(x => x.Id) // it will be added
             .ThenBy(x => x.Name, false) // it won't be added
             .ThenByDescending(x => x.Email); // since the parent is not added, this also will be discarded
    }
}
```
