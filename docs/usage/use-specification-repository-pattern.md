---
layout: default
title: How to use Specifications with the Repository Pattern
parent: Usage
nav_order: 3
---

# How to use Specifications with the Repository Pattern

If you're inclined to use the [Repository Pattern](https://deviq.com/design-patterns/repository-pattern), the specifications can help eliminate the common pain points and reduce the number of methods in the repositories. This library exposes an `ISpecificationEvaluator` evaluator implementation for EF Core and EF 6, which can be utilized to build the desired queries. The default evaluator implementation is a stateless object, and a singleton instance can be retrieved through `SpecificationEvaluator.Default`. 

Let's assume we have the following entity.

```csharp
public class Hero
{
    public string Name { get; set; }
    public string SuperPower { get; set; }
    public bool IsAlive { get; set; }
    public bool IsAvenger { get; set; }
}
```

We can implement a repository for Hero as follows. Instead of having multiple methods (e.g. GetAllAliveHeroes, GetAllAvengerHeroes, etc.), we may have a single method that accepts a specification as a parameter.

```csharp

public interface IHeroRepository
{
    List<Hero> ListHeroes(Specifcation<Hero> specification);
}

public class HeroRepository : IHeroRepository
{
    private readonly HeroDbContext _dbContext;

    public HeroRepository(HeroDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Hero> ListHeroes(ISpecification<Hero> spec)
    {
        var query = SpecificationEvaluator.Default.GetQuery(_dbContext.Heroes, spec);
        return query.ToList();
    }
}
```

Once we have that in place, we may create and use various specifications to retrieve the desired data from the repository.

```csharp
public class AliveHeroesSpec : Specification<Hero>
{
    public AliveHeroesSpec()
    {
        Query.Where(h => h.IsAlive == true);
    }
}

public class AliveAvengerHeroesSpec : Specification<Hero>
{
    public AliveHeroesSpec()
    {
        Query.Where(h => h.IsAlive == true && x.IsAvenger == true);
    }
}
```

```
var aliveHeroes = _repository.ListHeroes(new AliveHeroesSpec());
var aliveAvengerHeroes = _repository.ListHeroes(new AliveAvengerHeroesSpec());
```

## Further Reading

For more information on the Repository Pattern and the sample generic implementation included in this package, see the [How to use the Built In Abstract Repository](./use-built-in-abstract-repository.md) tutorial.

You can also see [this sample app in the Specification repo](https://github.com/ardalis/Specification/blob/main/samples/Ardalis.Sample.App3/) that shows how to define a custom RepositoryBase, separate IRepository and IReadRepository interfaces, and pagination constructs.