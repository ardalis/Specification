using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SpecificationImplementationEvaluatorGetQuery
    {
        private int _testId = 123;

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
            var spec = new ItemWithIdSpecification(_testId);

            var result = SpecificationEvaluator<TestItem>.GetQuery(
                GetTestListOfItems()
                    .AsQueryable(), 
                spec);

            var result1 = result.Single();

            Assert.Equal(_testId, result1.Id);
        }

        private List<TestItem> GetTestListOfItems()
        {
            return new List<TestItem>
            {
                new TestItem{ Id = 1},
                new TestItem{ Id = 2},
                new TestItem{ Id = 3},
                new TestItem{ Id = _testId}
            };
        }
    }
}
