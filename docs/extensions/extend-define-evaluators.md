---
layout: default
title: How to Define Your Own Evaluators
parent: Extensions
nav_order: 2
---

# How to Define Your Own Evaluators

The evaluation of specifications is handled by the `SpecificationEvaluator`, which delegates the work to multiple `IEvaluator` instances, each responsible for a specific feature or operator (e.g., Where, Search, Include, etc.). This design promotes composability and makes the evaluation pipeline easy to extend with custom behavior.

1. Create a Custom Partial Evaluator

Define your partial evaluator by implementing the `IEvaluator` interface. Most evaluators are stateless, so you might want to expose them as singleton instances to reduce allocations.

```csharp
public class MyPartialEvaluator : IEvaluator
{
    private MyPartialEvaluator() { }
    public static MyPartialEvaluator Instance { get; } = new MyPartialEvaluator();
    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        // Write your desired implementation
        return query;
    }
}
```

2. Create a Custom Specification Evaluator

Inherit from the `SpecificationEvaluator` class and add your custom evaluator to the evaluator list. By community request, the `Evaluators` property is exposed as a `List`, allowing you to insert or remove evaluators as needed. As previously stated, it might be wise to expose it as a singleton instance.

```csharp

public class MySpecificationEvaluator : SpecificationEvaluator
{
    public static MySpecificationEvaluator Instance { get; } = new MySpecificationEvaluator();

    private MySpecificationEvaluator() : base()
    {
        Evaluators.Add(MyPartialEvaluator.Instance);
    }
}
```

3. Register the Evaluator in Your Repository

The base `Repository<>` implementation includes a constructor overload that accepts a custom `ISpecificationEvaluator`. Pass your custom evaluator as follows.

```csharp
public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(AppDbContext dbContext)
      : base(dbContext, MySpecificationEvaluator.Instance)
    {
    }
}
```

## References

- [Enabled by PR 328](https://github.com/ardalis/Specification/pull/328)