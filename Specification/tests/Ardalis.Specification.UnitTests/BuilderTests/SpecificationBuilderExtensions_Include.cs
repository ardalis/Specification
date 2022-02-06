using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
  public class SpecificationBuilderExtensions_Include
  {
    [Fact]
    public void AddsNothingToList_GivenNoIncludeExpression()
    {
      var spec = new StoreEmptySpec();

      spec.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsNothingToList_GivenIncludeExpressionWithFalseCondition()
    {
      var spec = new CompanyByIdWithFalseConditions(1);

      spec.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsIncludeExpressionInfoToListWithTypeInclude_GivenIncludeExpression()
    {
      var spec = new StoreIncludeAddressSpec();

      spec.IncludeExpressions.Should().ContainSingle();
      spec.IncludeExpressions.Single().Type.Should().Be(IncludeTypeEnum.Include);
    }
  }
}
