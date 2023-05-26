---
layout: default
title: How to extend or define your own evaluators
parent: Extensions
nav_order: 3
---

How to extend or define your own evaluators.

## Evaluators

Evaluators are used within the specification to compose the query that will be executed. You can add your own evaluators to extend the behavior of the base Specification class.

Here is an example:

```csharp
public class MyPartialEvaluator : IEvaluator
{
  private MyPartialEvaluator () { }
  public static MyPartialEvaluator Instance { get; } = new MyPartialEvaluator();

  public bool IsCriteriaEvaluator { get; } = true;

  public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
  {
    // Write your desired implementation

    return query;
  }
}

public class MySpecificationEvaluator : SpecificationEvaluator
{
  public static MySpecificationEvaluator Instance { get; } = new MySpecificationEvaluator();

  private MySpecificationEvaluator() : base()
  {
    Evaluators.Add(MyPartialEvaluator.Instance);
  }
}
```

To use the evaluator, you would pass it into your repository implementation's constructor:

```csharp
public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
  public Repository(AppDbContext dbContext) 
    : base(dbContext, MySpecificationEvaluator.Instance)
  {
  }
}
```

Of course you would also need to register the service in `Program.cs`:

```csharp
builder.Services.AddScoped<ISpecificationEvaluator, MySpecificationEvaluator>();
```

## References

- [Enabled by PR 328](https://github.com/ardalis/Specification/pull/328)