
[![NuGet](https://img.shields.io/nuget/v/Ardalis.Specification.svg)](https://www.nuget.org/packages/Ardalis.Specification)[![NuGet](https://img.shields.io/nuget/dt/Ardalis.Specification.svg)](https://www.nuget.org/packages/Ardalis.Specification)
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

Base class with tests for adding specifications to a DDD model. Also includes a default generic Repository base class with support for EF6 and EF Core. Currently used in Microsoft reference application [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb), which is the best place to see it in action, as well as the [Clean Architecture solution template](https://github.com/ardalis/cleanarchitecture). Check out Steve "ardalis" Smith's associated (free!) eBook, [Architecting Modern Web Applications with ASP.NET Core and Azure](https://aka.ms/webappebook), as well.

## Documentation

### [Read the Documentation](https://ardalis.github.io/Specification/)

### Videos

🎥 [Watch What's New in v5 of Ardalis.Specification](https://www.youtube.com/watch?v=gT72mWdD4Qo&ab_channel=Ardalis)

🎥 [Watch an Overview of the Pattern and this Package](https://www.youtube.com/watch?v=BgWWbBUWyig)

## Version 7 Release Notes

Version 7 is now available on NuGet.org! We have had a lot of confusion about the need to have the version of Ardalis.Specification (and/or the EF6/EFCore packages) match the consuming project's version of .NET. We intend to version this package more frequently in the near future to make it clear that it need not match.

### Breaking Changes

* Updated projects, drop support for old TFMs. by @fiseni in https://github.com/ardalis/Specification/pull/326

### Other updates

* Patch 2 by @davidhenley in https://github.com/ardalis/Specification/pull/283
* Fix `Just the Docs` link in docs home page by @snowfrogdev in https://github.com/ardalis/Specification/pull/293
* Update url path by @ta1H3n in https://github.com/ardalis/Specification/pull/303
* Implement SelectMany support by @amdavie in https://github.com/ardalis/Specification/pull/320
* Add two methods for consuming repositories in scenarios where repositories could be longer lived (e.g. Blazor component Injections) by @jasonsummers in https://github.com/ardalis/Specification/pull/289
* Added support for AsAsyncEnumerable by @nkz-soft in https://github.com/ardalis/Specification/pull/316
* Lamadelrae/doc faq ef versions by @Lamadelrae in https://github.com/ardalis/Specification/pull/324
* Update the search feature to generate parameterized query. by @fiseni in https://github.com/ardalis/Specification/pull/327
* Add support for extending default evaluator list by @fiseni in https://github.com/ardalis/Specification/pull/328
* Ardalis/cleanup by @ardalis in https://github.com/ardalis/Specification/pull/332

## Version 6 Release Notes

See [Releases](https://github.com/ardalis/Specification/releases/tag/v6.1.0)

## Sample Usage

The Specification pattern pulls query-specific logic out of other places in the application where it currently exists. For applications with minimal abstraction that use EF Core directly, the specification will eliminate `Where`, `Include`, `Select` and similar expressions from almost all places where they're being used. In applications that abstract database query logic behind a `Repository` abstraction, the specification will typically eliminate the need for many custom `Repository` implementation classes as well as custom query methods on `Repository` implementations. Instead of many different ways to filter and shape data using various methods, the same capability is achieved with few core methods.

Example implementation in your repository using specifications

```c#
public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
{
	return await ApplySpecification(specification).ToListAsync(cancellationToken);
}

private IQueryable<T> ApplySpecification(ISpecification<T> specification)
{
	return SpecificationEvaluator.Default.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
}
```

Now to use this method, the calling code simply instantiates and passes the appropriate specification.

```c#
var spec = new CustomerByNameSpec("customerName");
var customers = await _repository.ListAsync(spec, cancellationToken);
```
Specifications should be defined in an easily-discovered location in the application, so developers can easily reuse them. The use of this pattern helps to eliminate many commonly duplicated lambda expressions in applications, reducing bugs associated with this duplication.

We're shipping a built-in repository implementation [RepositoryBase](https://github.com/ardalis/Specification/blob/main/Specification.EntityFrameworkCore/src/Ardalis.Specification.EntityFrameworkCore/RepositoryBaseOfT.cs), ready to be consumed in your apps. You can use it as a reference and create your own custom repository implementation.

## Running the tests

This project needs a database to test, since a lot of the tests validate that a specification is translated from LINQ to SQL by EF Core. To run the tests, we're using docker containers, including a docker-hosted SQL Server instance. You run the tests by simply running `RunTests.bat` or `RunTests.sh`.

## Reference

Some free video streams in which this package has been developed and discussed on [YouTube.com/ardalis](http://youtube.com/ardalis?sub_confirmation=1).

- [Reviewing the Specification Pattern and NuGet Package with guest Fiseni](https://www.youtube.com/watch?v=BgWWbBUWyig&t=315s&ab_channel=Ardalis) 6 Nov 2020
- [Open Source .NET Development with Specification and Other Projects](https://www.youtube.com/watch?v=zP_279p2D9w) 14 Jan 2020
- [Updating Specification and GuardClauses Nuget Packages / GitHub Samples](https://www.youtube.com/watch?v=kCeRJj2H1RQ) 20 Nov 2019
- [Ardalis - Working on the Ardalis.Specification and EF Extensions GitHub projects](https://www.youtube.com/watch?v=PbHic9Ndqoc) 16 Aug 2019
- [Working on the Ardalis.Specification Nuget Package and Integration Tests](https://www.youtube.com/watch?v=Ia3zb6-2LuY) 23 July 2019

Pluralsight resources:

- [Design Patterns Library - Specification](https://www.pluralsight.com/courses/patterns-library)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)
- [Specification Pattern in C#](https://www.pluralsight.com/courses/csharp-specification-pattern)
