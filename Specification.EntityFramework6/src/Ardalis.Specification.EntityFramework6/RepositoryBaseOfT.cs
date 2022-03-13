using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ardalis.Specification.EntityFramework6
{
  /// <inheritdoc/>
  public abstract class RepositoryBase<T> : ReadRepositoryBase<T> where T : class
  {
    private readonly DbContext dbContext;
    private readonly ISpecificationEvaluator specificationEvaluator;

    public RepositoryBase(DbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default)
    {
    }

    /// <inheritdoc/>
    public RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
      : base(dbContext, specificationEvaluator)
    {
      this.dbContext = dbContext;
      this.specificationEvaluator = specificationEvaluator;
    }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
      dbContext.Set<T>().Add(entity);

      await SaveChangesAsync(cancellationToken);

      return entity;
    }
    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().AddRange(entities);

        await SaveChangesAsync(cancellationToken);

        return entities;
    }
    
    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
      dbContext.Entry(entity).State = EntityState.Modified;

      await SaveChangesAsync(cancellationToken);
    }
    
    /// <inheritdoc/>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
      dbContext.Set<T>().Remove(entity);

      await SaveChangesAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
      dbContext.Set<T>().RemoveRange(entities);

      await SaveChangesAsync(cancellationToken);
    }
    
    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return await dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
