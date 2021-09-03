using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator
    {
        // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
        public static InMemorySpecificationEvaluator Default { get; } = new InMemorySpecificationEvaluator();

        private readonly List<IInMemoryEvaluator> evaluators = new List<IInMemoryEvaluator>();

        public InMemorySpecificationEvaluator()
        {
            this.evaluators.AddRange(new IInMemoryEvaluator[]
            {
                WhereEvaluator.Instance,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance
            });
        }
        public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }

        public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification)
        {
            _ = specification.Selector ?? throw new SelectorNotFoundException();

            var baseQuery = Evaluate(source, (ISpecification<T>)specification);

            var resultQuery = baseQuery.Select(specification.Selector.Compile());

            return specification.PostProcessingAction == null
                ? resultQuery
                : specification.PostProcessingAction(resultQuery);
        }

        public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
        {
            if (specification.SearchCriterias.Count() > 0)
            {
                throw new NotSupportedException("The specification contains Search expressions and can't be evaluated with in-memory evaluator.");
            }

            foreach (var evaluator in evaluators)
            {
                source = evaluator.Evaluate(source, specification);
            }

            return specification.PostProcessingAction == null
                ? source
                : specification.PostProcessingAction(source);
        }
    }
}
