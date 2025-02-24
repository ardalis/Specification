namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// Generates new instances of <typeparamref name="TRepository"/> to encapsulate the 'Unit of Work' pattern
/// in scenarios where injected types may be long-lived (e.g. Blazor)
/// </summary>
/// <typeparam name="TRepository">
/// The Interface of the Repository to be generated.
/// </typeparam>
public interface IRepositoryFactory<TRepository>
{
    /// <summary>
    /// Generates a new repository instance
    /// </summary>
    /// <returns>The generated repository instance</returns>
    public TRepository CreateRepository();
}
