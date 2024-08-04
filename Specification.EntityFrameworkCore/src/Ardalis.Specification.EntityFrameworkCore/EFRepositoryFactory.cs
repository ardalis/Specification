using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRepository">The Interface of the repository created by this Factory</typeparam>
/// <typeparam name="TConcreteRepository">
/// The Concrete implementation of the repository interface to create
/// </typeparam>
/// <typeparam name="TContext">The DbContext derived class to support the concrete repository</typeparam>
/// <remarks>
/// Initialises a new instance of the EFRepositoryFactory
/// </remarks>
/// <param name="dbContextFactory">The IDbContextFactory to use to generate the TContext</param>
public class EFRepositoryFactory<TRepository, TConcreteRepository, TContext>(
    IDbContextFactory<TContext> dbContextFactory) : IRepositoryFactory<TRepository>
  where TConcreteRepository : TRepository
  where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory = dbContextFactory;

    /// <inheritdoc />
    public TRepository CreateRepository()
    {
        var args = new object[] { _dbContextFactory.CreateDbContext() };
        return (TRepository)Activator.CreateInstance(typeof(TConcreteRepository), args)!;
    }
}
