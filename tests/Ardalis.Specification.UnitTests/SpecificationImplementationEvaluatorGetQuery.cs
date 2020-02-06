using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SpecificationImplementationEvaluatorGetQuery
    {
        private const int TestId = 123;

        private class TestItem : IEntity<int>
        {
            public int Id { get; set; }
        }
        private class ItemWithIdSpecification : BaseSpecification<TestItem>
        {
            public ItemWithIdSpecification(int id) : base(i => i.Id == id)
            {
            }
        }

        [Fact]
        public void ReturnsEntityWithId()
        {
            var spec = new ItemWithIdSpecification(TestId);

            var result = SpecificationEvaluator<TestItem,int>.GetQuery(
                GetTestListOfItems()
                    .AsQueryable(), 
                spec).Single();

            Assert.Equal(TestId, result.Id);
        }

        private IEnumerable<TestItem> GetTestListOfItems()
        {
            return new List<TestItem>
            {
                new TestItem{ Id = 1},
                new TestItem{ Id = 2},
                new TestItem{ Id = 3},
                new TestItem{ Id = TestId}
            };
        }
    }
}
