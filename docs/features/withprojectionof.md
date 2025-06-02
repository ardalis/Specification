---
layout: default
title: WithProjectionOf
nav_order: 3
has_children: false
parent: Features
---

# WithProjectionOf Feature

The `WithProjectionOf` extension method allows you to create a new specification by reusing an existing specification's filtering, ordering, and other query logic, but apply a projection from a different specification (e.g., a different `Select` or `SelectMany` clause). It returns a new combined specification while the input specifications remain unchanged. 

This is useful in various scenarios:
- You want to project to a different shapes of data (DTOs, VMs, etc.) from the same base query logic, without duplicating the logic.
- You want to project to a specific shape given different base query logic, without duplicating the logic.

## Usage 1

Suppose you have a specification that defines filtering, but you want to project the results into different DTO models:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}

public class CustomerDto1
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class CustomerDto2
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}

public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string name)
    {
        Query.Where(x => x.Name == name);
    }
}

public class CustomerToCustomerDto1Spec : Specification<Customer, CustomerDto1>
{
    public CustomerToCustomerDto1Spec()
    {
        Query.Select(x => new CustomerDto1
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}

public class CustomerToCustomerDto2Spec : Specification<Customer, CustomerDto2>
{
    public CustomerToCustomerDto2Spec()
    {
        Query.Select(x => new CustomerDto2
        {
            Id = x.Id,
            Name = x.Name,
            Address = x.Address
        });
    }
}
```

We may combine them as follows. It creates new `Specification<Customer, CustomerDto1>` and `Specification<Customer, CustomerDto2>` specifications, with the state of `CustomerSpec` and only the projection of `CustomerDto` specifications. The input specifications remain unchanged.

```csharp
var customerSpec = new CustomerSpec("John");
var customerDto1Spec = new CustomerToCustomerDto1Spec();
var customerDto2Spec = new CustomerToCustomerDto2Spec();

var newSpec1 = customerSpec.WithProjectionOf(customerDto1Spec);
var newSpec2 = customerSpec.WithProjectionOf(customerDto2Spec);
```

## Usage 2

Suppose you have two specifications that defines different filtering criteria, but you want to project the results into a specific DTO model. 

```csharp
public class Customer
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
}

public class CustomerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class CustomerSpec1 : Specification<Customer>
{
    public CustomerSpec1(string name)
    {
        Query.Where(x => x.FirstName == name);
    }
}

public class CustomerSpec2 : Specification<Customer>
{
    public CustomerSpec2(string lastName)
    {
        Query.Where(x => x.LastName == lastName);
    }
}

public class CustomerToCustomerDtoSpec : Specification<Customer, CustomerDto>
{
    public CustomerToCustomerDtoSpec()
    {
        Query.Select(x => new CustomerDto
        {
            Id = x.Id,
            Name = x.FirstName
        });
    }
}
```

Now, we may combine them as follows. It creates new `Specification<Customer, CustomerDto>` specifications, with the state of `Customer` specifications and only the projection of `CustomerToCustomerDtoSpec`. The input specifications remain unchanged.

```csharp
var customerSpec1 = new CustomerSpec1("John");
var customerSpec2 = new CustomerSpec2("Smith");
var customerDtoSpec = new CustomerToCustomerDtoSpec();

var newSpec1 = customerSpec1.WithProjectionOf(customerDtoSpec);
var newSpec2 = customerSpec2.WithProjectionOf(customerDtoSpec);
```

---

**See also:**
- [Projection Specification](../usage/create-specifications.md#projection-specification)
- [Select Feature](./select.md)
