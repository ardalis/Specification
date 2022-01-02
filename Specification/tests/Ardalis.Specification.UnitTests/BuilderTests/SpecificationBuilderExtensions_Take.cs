using System;

using Xunit;
using FluentAssertions;

using Ardalis.Specification.UnitTests.Fixture.Specs;

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
            Action sutAction = () => new StoreDuplicateTakeSpec();

            sutAction.Should()
                .Throw<DuplicateTakeException>()
                .WithMessage("Duplicate use of Take(). Ensure you don't use Take() more than once in the same specification!");
        }
    }
}
