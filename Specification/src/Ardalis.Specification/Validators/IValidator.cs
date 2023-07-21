namespace Ardalis.Specification;

public interface IValidator
{
    bool IsValid<T>(T entity, ISpecification<T> specification);
}
