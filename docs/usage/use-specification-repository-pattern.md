---
layout: default
title: How to use Specifications with the Repository Pattern
parent: Usage
nav_order: 2
---

# How to use Specifications with the Repository Pattern

Specifications shine when combined with the [Repository Pattern](https://deviq.com/design-patterns/repository-pattern), a sample generic implementation of which is included in this NuGet package. For the purpose of this walkthrough, the repository can be thought of as a simple data access abstraction over a collection of entities. In this example, the entity for a `Hero` and it's repository interface are defined below.

```csharp
public class Hero
{
    public string Name { get; set; }
    public string SuperPower { get; set; }
    public bool IsAlive { get; set; }
    public bool IsAvenger { get; set; }
}

public interface IHeroRepository
{
    List<Hero> GetAllHeroes();
}

```

A Specification can be defined for the `Hero` entity when filtering is required for the collection of all heroes. Note any fields that are needed to filter the heroes are passed to the Specification's constructor where the query logic should be implemented. In this case, a Specification is defined that filters heroes by whether the hero is alive and is an Avenger.

```csharp
public class HeroByIsAliveAndIsAvengerSpec : Specification<Hero>
{
    public HeroByIsAliveAndIsAvengerSpec(bool isAlive, bool isAvenger)
    {
        Query.Where(h => h.IsAlive == isAlive && h.IsAvenger == isAvenger);
    }
}
```

With the Specification and Repository defined, it is now possible to define a `GetHeroes` method that can take a `HeroRepository` as a parameter along with the filtering conditions and produce a filtered collection of heroes. Applying the Repository to the Specification is done using the `Evaluate` method on the Specification class which takes a `IEnumerable<T>` as a parameter. This should mirror the kind of methods typically found on Controllers or [Api Endpoints](https://github.com/ardalis/ApiEndpoints) where the IHeroRepository might be supplied via Dependency Injection to the class's constructor rather than passed as a parameter.

```csharp
public IEnumerable<Hero> GetHeroes(IHeroRepository repository, bool isAlive, bool isAvenger)
{
    var specification = new HeroByIsAliveAndIsAvengerSpec(isAlive, isAvenger);

    return specification.Evaluate(repository.GetAllHeroes());
}
```

Suppose the data store behind the IHeroRepository has the following state and client code calls the `GetHeroes` as below. The value of result should be a collection containing only the Spider Man hero.

```table
|  Name          |  SuperPower  | IsAlive | IsAvenger |
| -------------- | ------------ | ------- | --------- |
| Batman         | Intelligence | true    | false     |
| Iron Man       | Intelligence | false   | true      |
| Spider Man     | Spidey Sense | true    | true      |
```

```csharp
var result = GetHeroes(repository: repository, isAlive: true, isAvenger: true);
```

## Further Reading

For more information on the Repository Pattern and the sample generic implementation included in this package, see the [How to use the Built In Abstract Repository](./use-built-in-abstract-repository.md) tutorial.
