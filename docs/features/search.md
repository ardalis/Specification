---
layout: default
title: Search
nav_order: 2
has_children: false
parent: Base Features
grand_parent: Features
---

# Search

The `Search` feature in specifications provides a way to add SQL-like pattern matching to your queries.
- The `Search` feature is supported by all provider packages, including in-memory evaluation.
- For in-memory evaluation, `Search` is translated to custom C# implementation of SQL `LIKE` operator.
- For ORM providers (like EF Core), it is translated to the appropriate SQL `LIKE` expression.

## How to Use Search

The `Search` method is an extension on `ISpecificationBuilder` and accepts:
- A property selector (e.g., `x => x.Name`)
- A search term (e.g., `"%foo%"` for contains, `"foo%"` for starts with, etc.)
- An optional `int group` to control how multiple search predicates are grouped and combined.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Search(x => x.Name, "%" + name + "%");
    }
}
```

## Grouping and Logical Operators

The `group` parameter is an optional parameter that allows you to control how search predicates are grouped and combined.

By default, multiple `Search` statements are grouped together and combined using **OR** logic. This means if any of the `Search` conditions match, the entity will be included in the result. If you want to combine `Search` conditions using **AND** logic, you must specify a different group for each `Search` statement.

In the following example, we use `Search` to match on any of the properties (OR logic).
```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string searchTerm)
    {
        Query
            .Search(x => x.Name, "%" + searchTerm + "%")
            .Search(x => x.Email, "%" + searchTerm + "%");
    }
}
```

In the following example, both `Search` conditions must match (AND logic) because they are in different groups.
```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string? name, string? email)
    {
        Query
            .Search(x => x.Name, "%" + name + "%", group: 1)
            .Search(x => x.Email, "%" + email + "%", group: 2);
    }
}
```

## Conditional Overloads
Like other builder methods, `Search` supports an overload with a `bool condition` parameter. If the condition is false, the search is not applied. This is useful for dynamic filtering.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string? name)
    {
        Query.Search(x => x.Name, "%" + name + "%", !string.IsNullOrEmpty(name));
    }
}
```

---

**Tip:** Use `Search` for flexible, user-driven filtering scenarios, such as search boxes or advanced filtering UIs. Use the `group` parameter to control whether multiple search conditions are combined with OR (default) or AND (by specifying different groups).
