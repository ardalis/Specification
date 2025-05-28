---
layout: default
title: How to use Specifications with In Memory Collections
parent: Usage
nav_order: 5
---

# How to use Specifications with In Memory Collections

You can use specifications on collections of objects in memory. This approach can be convenient when retrieving data doesn't require querying a remote or out of process data store like a database. If the process does require querying external persistence, refer to the practices for using a specification with a [Repository Pattern](./use-specification-repository-pattern.md) or [DbContext](./use-specification-dbcontext.md).

A specification can be applied to an in memory collection using the `Evaluate` method. This method takes a single parameter of type `IEnumerable<T>` representing the collection on which the specification will be applied to.

```csharp
public class CustomerByAgeSpec : Specification<Customer>
{
    public CustomerByAgeSpec(int age)
    {
        Query.Where(x => x.Age > age);
    }
}
```

```csharp
IEnumerable<Customer> customers = [];

var spec = new CustomerByAgeSpec(18);
IEnumerable<Customer> filteredCustomers = spec.Evaluate(customers);
```

<strong>Note</strong>

The in-memory evaluation will ignore the ORM specific features in the specification. Please refer to the [Features](../features/index.md) section for the list of base and ORM specific functions.