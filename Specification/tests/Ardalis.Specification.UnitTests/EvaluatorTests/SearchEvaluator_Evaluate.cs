using System.Collections.Generic;

namespace Ardalis.Specification.UnitTests;

public class SearchEvaluator_Evaluate
{
    private static readonly List<Person> _people = new()
  {
    new Person("James"),
    new Person("Robert"),
    new Person("Mary"),
    new Person("Linda"),
    new Person("Michael"),
    new Person("David"),
  };

    [Theory]
    [InlineData("%mes", 1)]
    [InlineData("%r%", 2)]
    [InlineData("_inda", 1)]
    [InlineData("M%", 2)]
    [InlineData("[RM]%", 3)]
    [InlineData("_[IA]%", 5)]
    public void ReturnsFilteredList_GivenSearchExpression(string searchTerm, int expectedCount)
    {
        var result = SearchEvaluator.Instance.Evaluate(_people, new PersonSpecification(searchTerm));

        result.Should().HaveCount(expectedCount);
    }
}

public class PersonSpecification : Specification<Person>
{
    public PersonSpecification(string searchTerm)
    {
        Query.Search(x => x.Name, searchTerm);
    }
}

public class Person
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
    }
}
