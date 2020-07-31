using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int StoreId { get; set; }
        public Store? Store { get; set; }
    }
}
