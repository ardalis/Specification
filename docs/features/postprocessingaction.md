---
layout: default
title: PostProcessingAction
nav_order: 8
has_children: false
parent: Base Features
grand_parent: Features
---

# PostProcessingAction

The `PostProcessingAction` allows you to define a delegate of type `Func<IEnumerable<T>, IEnumerable<T>>` that is applied after data is retrieved from an external source (e.g., a database). This enables in-memory transformations or filtering that cannot be handled by the underlying query provider.

The delegate itself is stored as part of the specification state, but it is executed within the repository or data access layer after the initial query completes.

```csharp
public class CompanySpec : Specification<Company>
{
    public CompanySpec(int countryId)
    {
        Query.Where(x => x.CountryId == countryId)
             .Include(x => x.Stores);

        Query.PostProcessingAction(companies =>
        {
            // Your custom in-memory operation on the result set.
            return companies;
        });
    }
}
```

Repository usage.

```csharp
public virtual async Task<List<T>> ListAsync(
    ISpecification<T> spec, 
    CancellationToken cancellationToken = default)
{
    var result = await SpecificationEvaluator.Default
        .GetQuery(_dbContext.Heroes, spec)
        .ToListAsync(cancellationToken);

    return spec.PostProcessingAction is null 
        ? result 
        : spec.PostProcessingAction(result).ToList();
}
```