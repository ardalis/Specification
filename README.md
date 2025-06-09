
[![NuGet](https://img.shields.io/nuget/v/Ardalis.Specification.svg)](https://www.nuget.org/packages/Ardalis.Specification)
[![Actions Status](https://github.com/ardalis/Specification/actions/workflows/ci.yml/badge.svg)](https://github.com/ardalis/Specification/actions/workflows/ci.yml)
[![Generic badge](https://img.shields.io/badge/Documentation-Ardalis.Specification-Green.svg)](https://ardalis.github.io/Specification/)

<a href="https://twitter.com/intent/follow?screen_name=ardalis">
    <img src="https://img.shields.io/twitter/follow/ardalis.svg?label=Follow%20@ardalis" alt="Follow @ardalis" />
</a> &nbsp; <a href="https://twitter.com/intent/follow?screen_name=fiseni">
    <img src="https://img.shields.io/twitter/follow/fiseni.svg?label=Follow%20@fiseni" alt="Follow @fiseni" />
</a> &nbsp; <a href="https://twitter.com/intent/follow?screen_name=nimblepros">
    <img src="https://img.shields.io/twitter/follow/nimblepros.svg?label=Follow%20@nimblepros" alt="Follow @nimblepros" />
</a>

[![Stars Sparkline](https://stars.medv.io/ardalis/specification.svg)](https://stars.medv.io/ardalis/specification)

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!
# Specification

A .NET library for building query specifications. Currently used in Microsoft reference application [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb), which is the best place to see it in action, as well as the [Clean Architecture solution template](https://github.com/ardalis/cleanarchitecture). Check out Steve "ardalis" Smith's associated (free!) eBook, [Architecting Modern Web Applications with ASP.NET Core and Azure](https://aka.ms/webappebook), as well.

## Documentation

### [Read the Documentation](https://ardalis.github.io/Specification/)

## Releases

The change log for `version 9` and the list of breaking changes can be found [here](https://github.com/ardalis/Specification/issues/427).

## Sample Usage

The Specification pattern pulls query-specific logic out of other places in the application where it currently exists.
- For applications with minimal abstraction that use EF Core directly, the specification will eliminate `Where`, `Include`, `Select` and similar expressions from almost all places where they're being used.
- In applications that abstract database query logic behind a `Repository` abstraction, the specification will typically eliminate the need for many custom `Repository` implementation classes as well as custom query methods on `Repository` implementations. Instead of many different ways to filter and shape data using various methods, the same capability is achieved with few core methods.

Example implementation in your repository using specifications

```charp
public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
{
    var query = SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>(), specification);
    return await query.ToListAsync(cancellationToken);
}
```

Now to use this method, the calling code simply instantiates and passes the appropriate specification.

```charp
var spec = new CustomerByNameSpec("customerName");
var customers = await _repository.ListAsync(spec, cancellationToken);
```
Specifications should be defined in an easily-discovered location in the application, so developers can easily reuse them. The use of this pattern helps to eliminate many commonly duplicated lambda expressions in applications, reducing bugs associated with this duplication.

We're shipping a built-in repository implementation [RepositoryBase](https://github.com/ardalis/Specification/blob/main/src/Ardalis.Specification.EntityFrameworkCore/RepositoryBaseOfT.cs), ready to be consumed in your apps. You can use it as a reference and create your own custom repository implementation.

## Reference

Please refer to the documentation for [related resources](https://specification.ardalis.com/related-resources/).

Pluralsight resources:

- [Design Patterns Library - Specification](https://www.pluralsight.com/courses/patterns-library)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)
- [Specification Pattern in C#](https://www.pluralsight.com/courses/csharp-specification-pattern)
