using System.Collections.Generic;

namespace Ardalis.Specification;

public class SpecificationValidator : ISpecificationValidator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided validators.
    public static SpecificationValidator Default { get; } = new SpecificationValidator();

    private readonly List<IValidator> _validators = new();

    public SpecificationValidator()
    {
        _validators.AddRange(new IValidator[]
        {
            WhereValidator.Instance,
            SearchValidator.Instance
        });
    }
    public SpecificationValidator(IEnumerable<IValidator> validators)
    {
        _validators.AddRange(validators);
    }

    public virtual bool IsValid<T>(T entity, ISpecification<T> specification)
    {
        foreach (var partialValidator in _validators)
        {
            if (partialValidator.IsValid(entity, specification) == false) return false;
        }

        return true;
    }
}
