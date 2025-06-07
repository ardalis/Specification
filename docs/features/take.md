---
layout: default
title: Take
nav_order: 5
has_children: false
parent: Base Features
grand_parent: Features
---

# Take

The `Take` feature in specifications mirrors LINQ’s `Take` method. It accepts an `int count` and limits the number of elements returned from the beginning of the result set.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(int take)
    {
        Query.Take(take);
    }
}
```

`Take `is often used in combination with [Skip](skip.md) to implement pagination, but it can also be used independently to cap the result size.

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

`Take` provides an overload that accepts a `bool condition`. If the condition evaluates to false, the operation is skipped. This is useful for dynamic scenarios, such as optional paging.

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
