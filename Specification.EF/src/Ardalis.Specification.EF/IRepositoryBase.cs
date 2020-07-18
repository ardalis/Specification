using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ardalis.Specification.EF
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task SaveChangesAsync();

        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync<TId>(TId id);
        Task<T> GetBySpecAsync(ISpecification<T> specification);
        Task<List<T>> ListAsync();
        Task<List<T>> ListAsync(ISpecification<T> specification);
        Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification);
        Task<int> CountAsync(ISpecification<T> specification);
    }
}
