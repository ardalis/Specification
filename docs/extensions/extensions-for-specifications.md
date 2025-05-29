---
layout: default
title: How to Write Specification Extensions
parent: Extensions
nav_order: 1
---

# How to Write Specification Extensions

The internal infrastructure of this library is designed to be extensible. Builders and evaluators are implemented in a composable manner to make customization straightforward. Out-of-the-box builder methods are implemented as extension methods themselves, making it easy to add new ones as your requirements evolve.

You create specifications by using the `Query` builder within the constructor to define conditions. `Query` is an instance of `ISpecificationBuilder`, and all builder methods are extension methods defined for this interface. By using the same approach, you may write generic extensions for `Specification<T>`, or even more commonly, extensions specific for a given entity.

Let's assume we have a custom ordering logic for `Customer` entity, and we want to reuse the logic in all customer specifications.

```csharp
public static class CustomerSpecificationExtensions
{
    public static IOrderedSpecificationBuilder<Customer> ApplyOrdering(
        this ISpecificationBuilder<Customer> builder, 
        string sortBy, 
        string orderBy)
    {
        var isAscending = !orderBy?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

        return sortBy switch
        {
          "FirstName" => isAscending 
              ? builder.OrderBy(x => x.FirstName) 
              : builder.OrderByDescending(x => x.FirstName),

          "LastName" => isAscending 
              ? builder.OrderBy(x => x.Surname) 
              : builder.OrderByDescending(x => x.Surname),

          _ => builder.OrderBy(x => x.Id)
        };
    }
}
```

You can now reuse this logic in any `Customer` specification.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string sortBy, string orderBy)
    {
        Query.ApplyOrdering(sortBy, orderBy);
    }
}
```

## Extending the specification state

The internal Specification state is built around the features provided by default. However, custom behaviors often require storing additional state, used by custom builder extensions and evaluators. To support this, `Specification` exposes the `Items` property, which is a `Dictionary<string, object>`. This is a common extensibility pattern in many libraries. 

Let’s walk through implementing a custom `IgnoreAutoIncludes` behavior. This also demonstrates generic extensions for `Specification<T>`.

🔍 Note: This feature is available out-of-the-box in the latest version, but this example demonstrates how to implement such behavior yourself.

- Create a specification extension.

```csharp
public static class SpecExtensions
{
    public static ISpecificationBuilder<T> IgnoreAutoIncludes<T>(this ISpecificationBuilder<T> builder) 
        where T : class
    {
        builder.Specification.Items.TryAdd("IgnoreAutoIncludes", true);
        return builder;
    }
}
```

- Create the evaluator for it, and pass it to the base repository. Refer to [How to Define Your Own Evaluators](./extend-define-evaluators.md) section for more details.

```csharp
public class IgnoreAutoIncludesEvaluator : IEvaluator
{
    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.Items.ContainsKey("IgnoreAutoIncludes"))
        {
            query = query.IgnoreAutoIncludes();
        }
        return query;
    }
}

public class MySpecificationEvaluator : SpecificationEvaluator
{
    public static MySpecificationEvaluator Instance { get; } = new MySpecificationEvaluator();

    public MySpecificationEvaluator() : base()
    {
        Evaluators.Add(new IgnoreAutoIncludesEvaluator());
    }
}

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(AppDbContext dbContext) 
        : base(dbContext, MySpecificationEvaluator.Instance)
    {
    }
}
```

- Use the extension from any specification.

```csharp
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec()
    {
        Query.IgnoreAutoIncludes();
    }
}
```