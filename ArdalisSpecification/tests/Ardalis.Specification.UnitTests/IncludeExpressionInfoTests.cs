using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class IncludeExpressionInfoTests
    {
        private readonly Expression<Func<Company, Country>> expr;

        public IncludeExpressionInfoTests()
        {
            expr = x => x.Country!;
        }

        [Fact]
        public void ThrowsArgumentNullException_GivenNullForExpressionBody()
        {
            Assert.Throws<ArgumentNullException>(() => new IncludeExpressionInfo(null!, expr.Parameters, typeof(Company), typeof(Country)));
        }

        [Fact]
        public void ThrowsArgumentNullException_GivenNullForParameters()
        {
            Assert.Throws<ArgumentNullException>(() => new IncludeExpressionInfo(expr.Body, null!, typeof(Company), typeof(Country)));
        }

        [Fact]
        public void ThrowsArgumentNullException_GivenNullForEntityType()
        {
            Assert.Throws<ArgumentNullException>(() => new IncludeExpressionInfo(expr.Body, expr.Parameters, null!, typeof(Country)));
        }

        [Fact]
        public void ThrowsArgumentNullException_GivenNullForPropertyType()
        {
            Assert.Throws<ArgumentNullException>(() => new IncludeExpressionInfo(expr.Body!, expr.Parameters, typeof(Company), null!));
        }

        [Fact]
        public void ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
        {
            Assert.Throws<ArgumentNullException>(() => new IncludeExpressionInfo(expr.Body!, expr.Parameters, typeof(Company), typeof(Country), null!));
        }
    }
}
