---
layout: default
title: Skip
nav_order: 4
has_children: false
parent: Base Features
grand_parent: Features
---

# Skip

The `Skip` feature in specifications behaves the same as LINQ’s `Skip` method. It accepts an `int count` parameter and skips the specified number of elements from the beginning of the query result.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(int skip)
    {
        Query.Skip(skip);
    }
}
```

`Skip` is typically used together with [Take](take.md) to implement pagination but can also be used independently when needed.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(PagingFilter filter)
    {
        Query.Skip(filter.Skip)
             .Take(filter.Take);
    }
}
```

## Conditional Overloads

The `Skip` method provides an overload that accepts a `bool condition`. If the condition evaluates to false, the skip operation is not applied. This is useful for dynamic scenarios, such as optional paging.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(PagingFilter filter)
    {
        Query.Skip(filter.Skip, filter.Skip > 0)
             .Take(filter.Take, filter.Take > 0);
    }
}
```
