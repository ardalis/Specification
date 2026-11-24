# Ardalis.Specification

**IMPORTANT** This is the last MIT-licensed release of this library. Future releases will require a (potentially free) commercial license. [Find out more](https://nimbletools.io/news/ardalis-nuget-packages/)

A .NET library for building query specifications.

A [specification](https://deviq.com/design-patterns/specification-pattern) pulls query logic (filtering, ordering, includes, paging, and projections) out of controllers, services, and custom repository methods into small, named, reusable classes. A handful of generic repository methods can then run any specification, which removes most duplicated LINQ expressions and custom query methods. Companion packages provide evaluators and a ready-to-use `RepositoryBase<T>` for EF Core (`Ardalis.Specification.EntityFrameworkCore`) and EF6 (`Ardalis.Specification.EntityFramework6`).

## Release Notes

This is the last MIT-licensed release of this library (see above).

- Updated to the latest supported .NET frameworks (.NET 10, .NET 11) with continued NetStandard2.0 support (#553)
  - **Breaking:** dropped the `net8.0` and `net9.0` targets (.NET 8 and 9 are out of support). Projects that can't use .NET 10+ get the `netstandard2.0` build of `Ardalis.Specification` instead.
  - **Breaking (Ardalis.Specification.EntityFrameworkCore):** now targets `net10.0` and `net11.0` only, and requires EF Core 10 or 11.
  - `Ardalis.Specification.EntityFramework6` keeps its `net472` target for .NET Framework apps.
- Updated all NuGet dependencies (#553)
- NuGet package now ships its own focused readme

## Usage

Define a specification:

```c#
using Ardalis.Specification;

public class ActiveCustomersByNameSpec : Specification<Customer>
{
    public ActiveCustomersByNameSpec(string name)
    {
        Query.Where(c => c.IsActive && c.Name.StartsWith(name));
        Query.OrderBy(c => c.Name);
        Query.Include(c => c.Addresses);
    }
}
```

Then pass it to a repository:

```c#
// RepositoryBase<T> comes from Ardalis.Specification.EntityFrameworkCore
public class Repository<T>(AppDbContext dbContext)
    : RepositoryBase<T>(dbContext), IRepositoryBase<T> where T : class
{
}

var spec = new ActiveCustomersByNameSpec("Ard");
List<Customer> customers = await repository.ListAsync(spec, cancellationToken);
```

See the [Ardalis.Specification documentation](https://specification.ardalis.com/) for the full usage guide.

## Roadmap

This is our roadmap for this package:

### Version 10.x

This version. The last MIT-licensed package.

### Version 15.x

The next version. It will have minimal changes from 10.x but will have a dual license and a compile-time license check and any planned breaking changes will be marked `[Obsolete]` if possible to signal what's coming. It will also be released both as `Ardalis.Specification` and as `NimbleTools.Specification` but with identical behavior to make upgrades as easy as possible, both now and in the future.

Why the jump from 10 to 15? We want to consolidate the 3 Ardalis packages that are moving to commercial licensing (GuardClauses, SmartEnum, Specification) and, [like Aspire](https://github.com/microsoft/aspire/discussions/11824#discussioncomment-14938556), we want to keep our version numbers separated from .NET's version numbers. Starting at 15 for all 3 lets us do that.

### Version 16.x

This is where we expect to introduce potentially breaking changes as we rethink, refactor, and improve the implementation of the library. We will not break the API without a good reason, but there are some inconsistencies and places where we can improve the usability and sometimes the performance and this will be the time to make such changes.

Stay informed of changes by adding your email here: <https://nimbletools.io/updates>
