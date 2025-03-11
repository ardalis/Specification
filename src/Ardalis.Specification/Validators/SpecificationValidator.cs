namespace Ardalis.Specification;

public class SpecificationValidator : ISpecificationValidator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided validators.
    public static SpecificationValidator Default { get; } = new SpecificationValidator();

    protected List<IValidator> Validators { get; }

    public SpecificationValidator()
    {
        Validators =
        [
            WhereValidator.Instance,
            SearchValidator.Instance
        ];
    }

    public SpecificationValidator(IEnumerable<IValidator> validators)
    {
        Validators = validators.ToList();
    }

    public virtual bool IsValid<T>(T entity, ISpecification<T> specification)
    {
        foreach (var partialValidator in Validators)
        {
            if (partialValidator.IsValid(entity, specification) == false) return false;
        }

        return true;
    }
}
