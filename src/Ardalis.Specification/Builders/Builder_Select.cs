namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds a Select clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="selector">The selector expression.</param>
    public static void Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TResult>> selector)
    {
        builder.Specification.Selector = selector;
    }

    /// <summary>
    /// Adds a SelectMany clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="selector">The selector expression.</param>
    public static void SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        builder.Specification.SelectorMany = selector;
    }
}
