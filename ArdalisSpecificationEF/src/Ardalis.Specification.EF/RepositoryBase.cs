using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Specification.EntityFrameworkCore
{
    /// <inheritdoc/>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
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
        public async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);

            await SaveChangesAsync();

            return entity;
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbContext.Set<T>().RemoveRange(entities);

            await SaveChangesAsync();
        }
        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        /// <inheritdoc/>
        public async Task<T?> GetByIdAsync<TId>(TId id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        /// <inheritdoc/>
        public async Task<T?> GetBySpecAsync(ISpecification<T> specification)
        {
            return (await ListAsync(specification)).FirstOrDefault();
        }
        /// <inheritdoc/>
        public async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification)
        {
            return (await ListAsync(specification)).FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<List<T>> ListAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }
        /// <inheritdoc/>
        public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync();

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        /// <summary>
        /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
        /// <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
        protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
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
        protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}