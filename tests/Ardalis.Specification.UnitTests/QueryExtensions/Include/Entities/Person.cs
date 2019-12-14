using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.QueryExtensions.Include.Entities
{
    class Person
    {
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Book FavouriteBook { get; set; }
        public List<Person> Friends { get; set; }

        public string GetQuote()
        {
            return string.Empty;
        }
    }
}
