using Ardalis.Specification.QueryExtensions.Include;
using Ardalis.Specification.UnitTests.QueryExtensions.Include.Builders;
using Ardalis.Specification.UnitTests.QueryExtensions.Include.Entities;
using System.Linq;
using Xunit;

namespace Ardalis.Specification.UnitTests.QueryExtensions.Include
{
    public class IncludeQueryTests
    {
        private readonly IncludeQueryBuilder _includeQueryBuilder = new IncludeQueryBuilder();

        [Fact]
        public void Include_SimpleType_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();

            // There may be ORM libraries where including a simple type makes sense.
            var newIncludeQuery = includeQuery.Include(b => b.Title);

            Assert.Contains(newIncludeQuery.Paths, path => path == nameof(Book.Title));
        }

        [Fact]
        public void Include_Function_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();

            // This include does not make much sense, but it should at least not modify paths.
            var newIncludeQuery = includeQuery.Include(b => b.GetNumberOfSales());

            // The resulting paths should not include number of sales.
            Assert.DoesNotContain(newIncludeQuery.Paths, path => path == nameof(Book.GetNumberOfSales));
        }

        [Fact]
        public void Include_Object_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var newIncludeQuery = includeQuery.Include(b => b.Author);

            Assert.Contains(newIncludeQuery.Paths, path => path == nameof(Book.Author));
        }

        [Fact]
        public void Include_Collection_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();

            var newIncludeQuery = includeQuery.Include(b => b.Author!.Friends);
            var expectedPath = $"{nameof(Book.Author)}.{nameof(Person.Friends)}";

            Assert.Contains(newIncludeQuery.Paths, path => path == expectedPath);
        }

        [Fact]
        public void Include_IncreasesNumberOfPathsByOne()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var numberOfPathsBeforeInclude = includeQuery.Paths.Count;

            var newIncludeQuery = includeQuery.Include(b => b.Author!.Friends);
            var numberOfPathsAferInclude = newIncludeQuery.Paths.Count;

            var expectedNumerOfPaths = numberOfPathsBeforeInclude + 1;

            Assert.Equal(expectedNumerOfPaths, numberOfPathsAferInclude);
        }

        [Fact]
        public void Include_DoesNotModifyAnotherPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var pathsBeforeInclude = includeQuery.Paths;

            var newIncludeQuery = includeQuery.Include(b => b.Author!.Friends);
            var pathsAfterInclude = newIncludeQuery.Paths;

            Assert.Subset(pathsAfterInclude, pathsBeforeInclude);
        }

        [Fact]
        public void ThenInclude_SimpleType_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            // There may be ORM libraries where including a simple type makes sense.
            var newIncludeQuery = includeQuery.ThenInclude(p => p.Age);
            var pathAfterInclude = newIncludeQuery.Paths.First();
            var expectedPath = $"{pathBeforeInclude}.{nameof(Person.Age)}";

            Assert.Equal(expectedPath, pathAfterInclude);
        }

        [Fact]
        public void ThenInclude_Function_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            // This include does not make much sense, but it should at least not modify the paths.
            var newIncludeQuery = includeQuery.ThenInclude(p => p.GetQuote());
            var pathAfterInclude = newIncludeQuery.Paths.First();

            Assert.Equal(pathBeforeInclude, pathAfterInclude);
        }

        [Fact]
        public void ThenInclude_Object_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            var newIncludeQuery = includeQuery.ThenInclude(p => p.FavouriteBook);
            var pathAfterInclude = newIncludeQuery.Paths.First();
            var expectedPath = $"{pathBeforeInclude}.{nameof(Person.FavouriteBook)}";

            Assert.Equal(expectedPath, pathAfterInclude);
        }

        [Fact]
        public void ThenInclude_Collection_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithObjectAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            var newIncludeQuery = includeQuery.ThenInclude(p => p.Friends);
            var pathAfterInclude = newIncludeQuery.Paths.First();
            var expectedPath = $"{pathBeforeInclude}.{nameof(Person.Friends)}";

            Assert.Equal(expectedPath, pathAfterInclude);
        }

        [Fact]
        public void ThenInclude_PropertyOverCollection_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithCollectionAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            var newIncludeQuery = includeQuery.ThenInclude(p => p.FavouriteBook);
            var pathAfterInclude = newIncludeQuery.Paths.First();
            var expectedPath = $"{pathBeforeInclude}.{nameof(Person.FavouriteBook)}";

            Assert.Equal(expectedPath, pathAfterInclude);
        }

        [Fact]
        public void ThenInclude_FunctionOverCollection_ReturnsIncludeQueryWithCorrectPath()
        {
            var includeQuery = _includeQueryBuilder.WithCollectionAsPreviousProperty();
            var pathBeforeInclude = includeQuery.Paths.First();

            var newIncludeQuery = includeQuery.ThenInclude(p => p.GetQuote());
            var pathAfterInclude = newIncludeQuery.Paths.First();

            Assert.Equal(pathBeforeInclude, pathAfterInclude);
        }
    }
}
