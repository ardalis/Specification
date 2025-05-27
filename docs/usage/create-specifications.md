---
layout: default
title: How to Create Specifications
parent: Usage
nav_order: 1
---

# How to Create Specifications

## Basic Specification

Create your specification by inheriting from `Specification<T>`, and use the `Query` builder in the constructor to define your conditions.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(int age)
    {
        Query
            .Where(x => x.Age > age)
            .Include(x => x.Addresses)
            .OrderBy(x => x.FirstName);
    }
}
```

## Single Result Specification

The library exposes an `ISingleResultSpecification<T>` marker interface. We also define a `SingleResultSpecification<T>` base class as a convenience.

```csharp
public class SingleResultSpecification<T> : Specification<T>, ISingleResultSpecification<T>
{
}
```

Create your specification by inheriting from `SingleResultSpecification<T>`.

```csharp
public class CustomerByIdSpec : SingleResultSpecification<Customer>
{
    public CustomerByIdSpec(int id)
    {
        Query.Where(c => c.Id == id);
    }
}
```

The `ISingleResultSpecification<T>` and `SingleResultSpecification<T>` are functionally dormant. They act just as a marker and the consumers may utilize them to further clarify the intent of the given specification. For instance, `Single` or `SingleOrDefault` repository methods are constrained to accept only single result specifications.

## Projection Specification

The specification can be used to project the result into a different type. Inherit from `Specification<T, TResult>` base class, where `TResult` is the type you want to project into. This offers strongly typed experience in the builder and during the evaluation.

```csharp
public class CustomerSpec : Specification<Customer, CustomerDto>
{
    public CustomerSpec(int age)
    {
        Query
            .Where(x => x.Age > age)
            .OrderBy(x => x.FirstName)
            .Select(x => new CustomerDto(x.Id, x.Name));
    }
}
```

From here, additional operators can be used to further refine the Specification. These operators follow LINQ syntax and are described in more detail in the [Features](../features/index.md) section.
