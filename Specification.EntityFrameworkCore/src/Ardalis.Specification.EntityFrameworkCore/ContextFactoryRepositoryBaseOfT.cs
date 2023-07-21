using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore;

public abstract class ContextFactoryRepositoryBaseOfT<TEntity, TContext> : IRepositoryBase<TEntity>
  where TEntity : class
  where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public ContextFactoryRepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory)
      : this(dbContextFactory, SpecificationEvaluator.Default)
    {
    }

    public ContextFactoryRepositoryBaseOfT(IDbContextFactory<TContext> dbContextFactory,
      ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextFactory = dbContextFactory;
        _specificationEvaluator = specificationEvaluator;
    }

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> SingleOrDefaultAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification,
      CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var queryResult = await ApplySpecification(specification, dbContext).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<TResult>> ListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var queryResult = await ApplySpecification(specification, dbContext).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext, true).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<TEntity>().CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await ApplySpecification(specification, dbContext, true).AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<TEntity>().AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        return ApplySpecification(specification, dbContext).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().Add(entity);

        await SaveChangesAsync(dbContext, cancellationToken);

        return entity;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().AddRange(entities);

        await SaveChangesAsync(dbContext, cancellationToken);

        return entities;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().Update(entity);

        await SaveChangesAsync(dbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().UpdateRange(entities);

        await SaveChangesAsync(dbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().Remove(entity);

        await SaveChangesAsync(dbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<TEntity>().RemoveRange(entities);

        await SaveChangesAsync(dbContext, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }

    public async Task<int> SaveChangesAsync(TContext dbContext, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Filters the entities  of <typeparamref name="TEntity"/>, to those that match the encapsulated query logic of the
    /// <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, TContext dbContext, bool evaluateCriteriaOnly = false)
    {
        return _specificationEvaluator.GetQuery(dbContext.Set<TEntity>().AsQueryable(), specification, evaluateCriteriaOnly);
    }

    /// <summary>
    /// Filters all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification, TContext dbContext)
    {
        return _specificationEvaluator.GetQuery(dbContext.Set<TEntity>().AsQueryable(), specification);
    }
}
