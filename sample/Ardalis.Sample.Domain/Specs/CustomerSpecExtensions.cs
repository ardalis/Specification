using Ardalis.Sample.Domain.Filters;
using Ardalis.Specification;

namespace Ardalis.Sample.Domain.Specs;

// Examples how to add extend specifications.
// These extensions are applied in Ardalis.Sample.Domain.Specs.CustomerSpec
public static class CustomerSpecExtensions
{
    // Let's assume we want to apply ordering for customers.
    // Conveniently, we can create add an extension method, and use it in any customer spec.
    public static ISpecificationBuilder<Customer> ApplyOrdering(this ISpecificationBuilder<Customer> builder, BaseFilter? filter = null)
    {
        // If there is no filter apply default ordering;
        if (filter is null) return builder.OrderBy(x => x.Id);

        // We want the "asc" to be the default, that's why the condition is reverted.
        var isAscending = !(filter.OrderBy?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false);

        return filter.SortBy switch
        {
            nameof(Customer.Name) => isAscending ? builder.OrderBy(x => x.Name) : builder.OrderByDescending(x => x.Name),
            nameof(Customer.Age) => isAscending ? builder.OrderBy(x => x.Age) : builder.OrderByDescending(x => x.Age),
            _ => builder.OrderBy(x => x.Id)
        };
    }

    // More complex scenario would be if we want to add a new feature.
    // Let's say we want to add a capability for query tags. We can utilize the Items dictionary in the specification to store the tag.
    // Once we have this in place, we would also need to add an evaluator in SpecificationEvaluator (check the example in Sample.App2)
    public static ISpecificationBuilder<Customer> TagWith(this ISpecificationBuilder<Customer> builder, string tag)
    {
        builder.Specification.Items.TryAdd("TagWith", tag);
        return builder;
    }
}
