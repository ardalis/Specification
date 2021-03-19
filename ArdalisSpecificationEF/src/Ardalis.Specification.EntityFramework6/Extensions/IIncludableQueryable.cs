using System.Linq;

namespace Ardalis.Specification.EntityFramework6
{
    public interface IIncludableQueryable<out TEntity> : IQueryable<TEntity>
    {
    }
}