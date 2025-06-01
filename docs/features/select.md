---
layout: default
title: Select
nav_order: 7
has_children: false
parent: Base Features
grand_parent: Features
---

# Select and SelectMany

The `Select` and `SelectMany` features behave the same as their LINQ counterparts. They are used to project, transform, or flatten query results.

## How to Use Select

The `Select` method is used to project elements into a new form. In the context of specifications, it's commonly used to map entities to DTOs, or select specific properties.

To use `Select`, your specification should inherit from `Specification<T, TResult>`, where `T` is the entity type and `TResult` is the projection type. This enables strongly typed experience in the builder and during the evaluation.

```csharp
public class CustomerSpec : Specification<Customer, CustomerDto>
{
    public CustomerSpec(int age)
    {
        Query.Where(x => x.Age > age)
             .Select(x => new CustomerDto(x.Id, x.Name));
    }
}
```

You can also project to a single property.

```csharp
public class CustomerSpec : Specification<Customer, string?>
{
    public CustomerSpec()
    {
        Query.Select(x => x.Name);
    }
}
```

## How to use SelectMany

The `SelectMany` method is used to flatten nested collections. It projects each element to an `IEnumerable` and flattens the results into a single sequence. In the following example, it will return a flat list of all `Address` objects across all `Customer` entities.

```csharp
public class CustomerSpec : Specification<Customer, Address>
{
    public CustomerSpec()
    {
        Query.SelectMany(c => c.Addresses);
    }
}
```

## Why Use Select and SelectMany?
- Reduces data transfer by returning only needed fields
- Supports projection to DTOs
- Enables efficient queries. The projection is applied at the query level, so only the selected fields are retrieved from the data source.
- `SelectMany` is ideal for flattening nested collections like child entities