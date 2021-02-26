using Ardalis.Specification.UnitTests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SpecificationEvaluator_GetQuery
    {
        private int _testId = 123;

        private class TestItem
        {
            public int Id { get; set; }
        }

        private class ItemWithIdSpecification : Specification<TestItem>
        {
            public ItemWithIdSpecification(int id)
            {
                Query.Where(x => x.Id == id);
            }
        }

        [Fact]
        public void ReturnsEntityWithId_GiveWhereExpression()
        {
            var spec = new ItemWithIdSpecification(_testId);

            var evaluator = new SpecificationEvaluator();
            var result = evaluator.GetQuery(GetTestListOfItems().AsQueryable(), spec).FirstOrDefault();

            Assert.Equal(_testId, result?.Id);
        }

        private List<TestItem> GetTestListOfItems()
        {
            return new List<TestItem>
            {
                new TestItem{ Id = 1},
                new TestItem{ Id = 2},
                new TestItem{ Id = _testId},
                new TestItem{ Id = 3}
            };
        }
    }
}
