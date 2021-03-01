using Ardalis.Specification;

namespace Ardalis.SampleApp.Core.Interfaces
{
    /// <inheritdoc/>
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
