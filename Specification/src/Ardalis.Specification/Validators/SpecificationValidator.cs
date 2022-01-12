using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public class SpecificationValidator : ISpecificationValidator
    {
        // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided validators.
        public static SpecificationValidator Default { get; } = new SpecificationValidator();

        private readonly List<IValidator> validators = new List<IValidator>();

        public SpecificationValidator()
        {
            this.validators.AddRange(new IValidator[]
            {
                WhereValidator.Instance,
                SearchValidator.Instance
            });
        }
        public SpecificationValidator(IEnumerable<IValidator> validators)
        {
            this.validators.AddRange(validators);
        }

        public virtual bool IsValid<T>(T entity, ISpecification<T> specification)
        {
            foreach (var partialValidator in validators)
            {
                if (partialValidator.IsValid(entity, specification) == false) return false;
            }

            return true;
        }
    }
}
