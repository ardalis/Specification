using Ardalis.Specification.QueryExtensions.Include;
using Ardalis.Specification.UnitTests.QueryExtensions.Include.Entities;
using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests.QueryExtensions.Include.Builders
{
    internal class IncludeQueryBuilder
    {
        public IncludeQuery<Person, List<Person>> WithCollectionAsPreviousProperty()
        {
            var pathMap = new Dictionary<IIncludeQuery, string>();
            var query = new IncludeQuery<Person, List<Person>>(pathMap);
            pathMap[query] = nameof(Person.Friends);

            return query;
        }

        public IncludeQuery<Book, Person> WithObjectAsPreviousProperty()
        {
            var pathMap = new Dictionary<IIncludeQuery, string>();
            var query = new IncludeQuery<Book, Person>(pathMap);
            pathMap[query] = nameof(Book.Author);

            return query;
        }
    }
}
