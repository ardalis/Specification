using Ardalis.Specification.QueryExtensions.Include;
using Ardalis.Specification.UnitTests.QueryExtensions.Include.Entities;
using Xunit;

namespace Ardalis.Specification.UnitTests.QueryExtensions.Include
{
    public class IncludeAggregatorTests
    {
        [Fact]
        public void Include_SimpleType_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeAggregator = new IncludeAggregator<Person>();

            // There may be ORM libraries where including a simple type makes sense.
            var includeQuery = includeAggregator.Include(p => p.Age);

            Assert.Contains(includeQuery.Paths, path => path == nameof(Person.Age));
        }

        [Fact]
        public void Include_Function_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeAggregator = new IncludeAggregator<Person>();

            // This include does not make much sense, but it should at least not modify the paths.
            var includeQuery = includeAggregator.Include(p => p.FavouriteBook.GetNumberOfSales());

            // The resulting paths should not include number of sales.
            Assert.DoesNotContain(includeQuery.Paths, path => path == nameof(Book.GetNumberOfSales));
        }

        [Fact]
        public void Include_Object_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeAggregator = new IncludeAggregator<Person>();
            var includeQuery = includeAggregator.Include(p => p.FavouriteBook.Author);

            Assert.Contains(includeQuery.Paths, path => path == $"{nameof(Person.FavouriteBook)}.{nameof(Book.Author)}");
        }

        [Fact]
        public void Include_Collection_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeAggregator = new IncludeAggregator<Book>();
            var includeQuery = includeAggregator.Include(o => o.Author.Friends);

            Assert.Contains(includeQuery.Paths, path => path == $"{nameof(Book.Author)}.{nameof(Person.Friends)}");
        }
    }
}
