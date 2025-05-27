---
layout: default
title: How to use the Built In Abstract Repository
parent: Usage
nav_order: 4
---

# How to use the Built In Abstract Repository

## Introduction

This library primarily provides facilities for creating and evaluating specifications. However, because specifications are often used alongside the repository pattern, it also includes base repository implementations.

It's important to note that application requirements can vary widely, so this library does not attempt to offer a one-size-fits-all solution. Instead, the base implementations are designed to serve as reference points or starting templates for your own customized repositories.

Define your `IRepository<>` and `Repository<>` by inheriting from the base constructs as shown below.

```csharp
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    // Define additional contracts if necessary.
}

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(DbContext dbContext) : base(dbContext)
    {
    }

    // Implement additional behavior if necessary.
    // All base methods are marked as virtual and can be overloaded.
}
```

Then register the repository in your dependency injection container.

```csharp
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

That’s it! You can now inject `IRepository<>` into your services and use it to query your data. The base repository exposes methods with canonical naming conventions that should be self-explanatory.

## Resources

An in-depth demo of a similar implementation of the Repository Pattern and `RepositoryBase<T>` can be found in the Repositories section of this [Pluralsight course](https://www.pluralsight.com/courses/domain-driven-design-fundamentals).
