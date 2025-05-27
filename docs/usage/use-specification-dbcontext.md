---
layout: default
title: How to use Specifications with a DbContext
parent: Usage
nav_order: 2
---

# How to use Specifications with a DbContext

Specifications can be applied to any `DbSet<>` or `IQueryable<>` source using the `WithSpecification` extension method. This extension method is defined in the `Ardalis.Specification.EntityFrameworkCore` and `Ardalis.Specification.EntityFramework6` plugin packages.

Let's assume we have the following setup.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(int age)
    {
        Query.Where(x => x.Age > age)
             .Include(x => x.Addresses)
             .OrderBy(x => x.FirstName);
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class SampleDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }
}
```

Apply the specification to a given `DbSet` as follows.

```csharp
var spec = new CustomerSpec(20);

var customer = await dbContext.Customers
    .WithSpecification(spec)
    .ToListAsync();
```

The `WithSpecification` extension methods returns the `IQueryable<>` source, and you may continue building the state. Having said that, you may chain multiple specifications as follows.

```csharp
var spec1 = new CustomerByAgeSpec(20);
var spec2 = new CustomerByLastNameSpec("Smith");

var customer = await dbContext.Customers
    .WithSpecification(spec1)
    .WithSpecification(spec2)
    .ToListAsync();
```
