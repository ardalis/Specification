using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Specification.EF
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext dbContext;
        private readonly ISpecificationEvaluator<T> specificationEvaluator;

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.specificationEvaluator = new SpecificationEvaluator<T>();
        }

        public RepositoryBase(DbContext dbContext, ISpecificationEvaluator<T> specificationEvaluator)
        {
            this.dbContext = dbContext;
            this.specificationEvaluator = specificationEvaluator;
        }

        public async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);

            await SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);

            await SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbContext.Set<T>().RemoveRange(entities);

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync<TId>(TId id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetBySpecAsync(ISpecification<T> specification)
        {
            return (await ListAsync(specification)).FirstOrDefault();
        }

        public async Task<List<T>> ListAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new Exception("Specification must have Selector defined.");

            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }


        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
        }
        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}
