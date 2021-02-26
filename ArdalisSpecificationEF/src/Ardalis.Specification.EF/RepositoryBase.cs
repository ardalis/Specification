using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Specification.EntityFrameworkCore
{
    /// <inheritdoc/>
    public abstract class RepositoryBase : IRepositoryBase
    {
        private readonly DbContext dbContext;
        private readonly ISpecificationEvaluator specificationEvaluator;

        public RepositoryBase(DbContext dbContext)
            : this(dbContext, SpecificationEvaluator.Default)
        {
        }

        /// <inheritdoc/>
        public RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
        {
            this.dbContext = dbContext;
            this.specificationEvaluator = specificationEvaluator;
        }

        /// <inheritdoc/>
        public virtual async Task<T> AddAsync<T>(T entity) where T : class
        {
            dbContext.Set<T>().Add(entity);

            await SaveChangesAsync();

            return entity;
        }
        /// <inheritdoc/>
        public virtual async Task UpdateAsync<T>(T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public virtual async Task DeleteAsync<T>(T entity) where T : class
        {
            dbContext.Set<T>().Remove(entity);

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public virtual async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            dbContext.Set<T>().RemoveRange(entities);

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<T?> GetByIdAsync<T, TId>(TId id) where T : class
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        /// <inheritdoc/>
        public virtual async Task<T?> GetBySpecAsync<T>(ISpecification<T> specification) where T : class
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }
        /// <inheritdoc/>
        public virtual async Task<TResult> GetBySpecAsync<T, TResult>(ISpecification<T, TResult> specification) where T : class
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<List<T>> ListAsync<T>() where T : class
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        /// <inheritdoc/>
        public virtual async Task<List<T>> ListAsync<T>(ISpecification<T> specification) where T : class
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }
        /// <inheritdoc/>
        public virtual async Task<List<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification) where T : class
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountAsync<T>(ISpecification<T> specification) where T : class
        {
            return await ApplySpecification(specification, true).CountAsync();
        }

        /// <summary>
        /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
        /// <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
        protected virtual IQueryable<T> ApplySpecification<T>(ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
        {
            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
        /// <summary>
        /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
        protected virtual IQueryable<TResult> ApplySpecification<T, TResult>(ISpecification<T, TResult> specification) where T : class
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}