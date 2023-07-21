using System;

namespace Ardalis.Specification.UnitTests;

public class SpecificationBuilderExtensions_Skip
{
    [Fact]
    public void SetsSkipProperty_GivenValue()
    {
        var skip = 1;

        var spec = new StoreNamesPaginatedSpec(skip, 10);

        spec.Skip.Should()
            .Be(skip);
    }

    [Fact]
    public void DoesNothing_GivenSkipWithFalseCondition()
    {
        var spec = new CompanyByIdWithFalseConditions(1);

        spec.Skip.Should().BeNull();
    }

    [Fact]
    public void ThrowsDuplicateSkipException_GivenSkipUsedMoreThanOnce()
    {
        Action sutAction = () => new StoreDuplicateSkipSpec();

        sutAction.Should()
            .Throw<DuplicateSkipException>()
            .WithMessage("Duplicate use of Skip(). Ensure you don't use Skip() more than once in the same specification!");
    }
}
