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
    public class SpecificationBuilderExtensions_Take
    {
        [Fact]
        public void SetsTakeProperty_GivenValue()
        {
            var take = 10;
            var spec = new StoreNamesPaginatedSpec(0, take);

            spec.Take.Should().Be(take);
        }

        [Fact]
        public void ThrowsDuplicateTakeException_GivenTakeUsedMoreThanOnce()
        {
            Assert.Throws<DuplicateTakeException>(() => new StoreDuplicateTakeSpec());
        }
    }
}
