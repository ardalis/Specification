using FluentAssertions;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SpecificationBuilderExtensions_OrderByDescending
    {
        [Fact]
        public void AddsNothingToList_GivenNoOrderExpression()
        {
            var spec = new StoreEmptySpec();

            spec.OrderExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsNothingToList_GivenOrderExpressionWithFalseCondition()
        {
            var spec = new CompanyByIdWithFalseConditions(1);

            spec.OrderExpressions.Should().BeEmpty();
        }

        [Fact]
        public void AddsOrderExpressionToListWithOrderByDescendingType_GivenOrderByDescendingExpression()
        {
            var spec = new StoresOrderedDescendingByNameSpec();

            spec.OrderExpressions.Should().ContainSingle();
            spec.OrderExpressions.Single().OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
        }
    }
}
