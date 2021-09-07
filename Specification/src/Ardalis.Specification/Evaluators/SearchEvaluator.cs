using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public class SearchEvaluator : IInMemoryEvaluator
    {
        private SearchEvaluator() { }
        public static SearchEvaluator Instance { get; } = new SearchEvaluator();

        public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
        {
            foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchGroup.Select(x => (x.Selector, x.SearchTerm));

                query = query.Where(x => criterias.Any(c => c.Selector.Compile()(x).Like(c.SearchTerm)));
            }

            return query;
        }
    }
}
