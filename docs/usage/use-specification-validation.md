---
layout: default
title: How to use Specifications for Validation
parent: Usage
nav_order: 6
---

# How to use Specifications for Validation

A specification can be used to validate a given entity using the `IsSatisfiedBy` method. This method takes a single parameter of type `T` and returns a `bool` result.

```csharp
public class AdultCustomerSpec : Specification<Customer>
{
    public AdultCustomerSpec()
    {
        Query.Where(x => x.Age >= 18);
    }
}
```

```csharp
var customer = new Customer
{
    Age = 20
};

var spec = new AdultCustomerSpec();
bool isAdult = spec.IsSatisfiedBy(customer);
```

<strong>Note</strong>

The validation will utilize only the following functions, and ignore all other conditions defined in the specification.
- `Where`
- `Search`