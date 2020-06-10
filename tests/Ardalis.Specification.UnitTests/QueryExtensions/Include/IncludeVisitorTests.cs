using Ardalis.Specification.QueryExtensions.Include;
using Ardalis.Specification.UnitTests.QueryExtensions.Include.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Ardalis.Specification.UnitTests.QueryExtensions.Include
{
    public class IncludeVisitorTests
    {
        [Fact]
        public void Visit_ExpressionWithSimpleType_ShouldSetCorrectPath()
        {
            var visitor = new IncludeVisitor();
            Expression<Func<Book, string>> expression = (book) => book.Author!.FirstName!;
            visitor.Visit(expression);

            var expectedPath = $"{nameof(Book.Author)}.{nameof(Person.FirstName)}";
            Assert.Equal(expectedPath, visitor.Path);
        }

        [Fact]
        public void Visit_ExpressionWithObject_ShouldSetCorrectPath()
        {
            var visitor = new IncludeVisitor();
            Expression<Func<Book, Book>> expression = (book) => book.Author!.FavouriteBook!;
            visitor.Visit(expression);

            var expectedPath = $"{nameof(Book.Author)}.{nameof(Person.FavouriteBook)}";
            Assert.Equal(expectedPath, visitor.Path);
        }

        [Fact]
        public void Visit_ExpressionWithCollection_ShouldSetCorrectPath()
        {
            var visitor = new IncludeVisitor();
            Expression<Func<Book, List<Person>>> expression = (book) => book.Author!.Friends!;
            visitor.Visit(expression);

            var expectedPath = $"{nameof(Book.Author)}.{nameof(Person.Friends)}";
            Assert.Equal(expectedPath, visitor.Path);
        }

        [Fact]
        public void Visit_ExpressionWithFunction_ShouldSetCorrectPath()
        {
            var visitor = new IncludeVisitor();
            Expression<Func<Book, string>> expression = (book) => book.Author!.GetQuote();
            visitor.Visit(expression);

            var expectedPath = nameof(Book.Author);
            Assert.Equal(expectedPath, visitor.Path);
        }
    }
}
