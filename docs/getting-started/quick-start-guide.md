---
layout: default
title: Quick Start Guide
parent: Getting Started
nav_order: 1
---

# Ardalis.Specification Quick Start Guide

1. Install Nuget-Package(s)

   a. Always required: [Ardalis.Specification](https://www.nuget.org/packages/Ardalis.Specification/)

   b. If you want to use it with EF Core also install the package [Ardalis.Specification.EntityFrameworkCore](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)

   c. Alternatively, if you want to use it with EF6 also install the package [Ardalis.Specification.EntityFramework6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)
   
2. Create a specification by inheriting from `Specification<T>`.

   ```csharp
    public class CustomerByLastNameSpec : Specification<Customer>
    {
        public CustomerByLastNameSpec(string lastName)
        {
            Query.Where(c => c.LastName == lastName);
        }
    }
   ```
   
3. Apply a specification to a `DbSet` or `IQueryable` source.

   ```csharp
    var spec = new CustomerByLastNameSpec("Smith");
    var customers = await _dbContext.Customers
        .WithSpecification(spec)
        .ToListAsync();
   ```

4. Alternatively, apply the specification using repositories. Learn how to create and use repositories in the following sections [How to use Specifications with the Repository Pattern](../usage/use-specification-repository-pattern.md) and [How to use the Built In Abstract Repository](../usage/use-built-in-abstract-repository.md)

   ```csharp
    var spec = new CustomerByLastNameSpec("Smith");
    var customers = await _customerRepo.ListAsync(spec);
   ```
   