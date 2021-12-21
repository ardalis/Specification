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
    public class SpecificationBuilderExtensions_Skip
    {
        [Fact]
        public void SetsSkipProperty_GivenValue()
        {
            var skip = 1;

            var spec = new StoreNamesPaginatedSpec(skip, 10);

            spec.Skip.Should().Be(skip);
            spec.IsPagingEnabled.Should().BeTrue();
        }

        [Fact]
        public void DoesNothing_GivenSkipWithFalseCondition()
        {
            var spec = new CompanyByIdWithFalseConditions(1);

            spec.Skip.Should().BeNull();
            spec.IsPagingEnabled.Should().BeFalse();
        }

        [Fact]
        public void ThrowsDuplicateSkipException_GivenSkipUsedMoreThanOnce()
        {
            Assert.Throws<DuplicateSkipException>(() => new StoreDuplicateSkipSpec());
        }
    }
}
